using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlphaVantage.Net.Core;
using AlphaVantage.Net.Stocks.Parsing.Exceptions;
using AlphaVantage.Net.Stocks.Parsing.JsonTokens;
using AlphaVantage.Net.Stocks.TimeSeries;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AlphaVantage.Net.Stocks.Parsing
{
    internal partial class DataParser
    {
        public TimeSeriesData ParseFxTimeSeries(JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var properties = jObject.Children().Select(ch => (JProperty)ch).ToArray();

                var metadataJson = properties.FirstOrDefault(p => p.Name == StockMetaDataJsonToken.MetaDataHeader);
                var timeSeriesJson =
                    properties.FirstOrDefault(p => p.Name.Contains(StockTimeSeriesJsonToken.TimeSeriesHeader));

                if (metadataJson == null || timeSeriesJson == null)
                    throw new TimeSeriesParsingException($"Unable to parse time-series json. Response: {jObject}");

                var result = new TimeSeriesData();

                EnrichWithFxMetadata(metadataJson, result);
                result.DataPoints = GetStockDataPoints(timeSeriesJson);

                return result;
            }
            catch (TimeSeriesParsingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TimeSeriesParsingException("Unable to parse data. See the inner exception for details", ex);
            }
        }

        private void EnrichWithFxMetadata([NotNull] JProperty metadataJson, [NotNull] TimeSeriesData timeSeriesData)
        {
            var metadatas = metadataJson.Children().Single();

            foreach (var metadataItem in metadatas)
            {
                var metadataProperty = (JProperty)metadataItem;
                var metadataItemName = metadataProperty.Name;
                var metadataItemValue = metadataProperty.Value.ToString();

                if (metadataItemName.Contains(FxMetaDataJsonToken.InformationToken))
                {
                    timeSeriesData.Type = GetTimeSeriesType(metadataItemValue);
                    timeSeriesData.IsAdjusted = IsAdjusted(metadataItemValue);
                }
                else if (metadataItemName.Contains(FxMetaDataJsonToken.RefreshTimeToken))
                {
                    var refreshTime = metadataItemValue.Replace("(end of day)", string.Empty).Trim().ParseDateTime();
                    timeSeriesData.LastRefreshedUtc = DateTime.SpecifyKind(refreshTime, DateTimeKind.Local);
                }
                else if (metadataItemName.Contains(FxMetaDataJsonToken.SymbolFrom))
                {
                    timeSeriesData.Symbol = metadataItemValue;
                }
                else if (metadataItemName.Contains(FxMetaDataJsonToken.SymbolTo))
                {
                    timeSeriesData.Market = metadataItemValue;
                }
            }
        }
    }
}
