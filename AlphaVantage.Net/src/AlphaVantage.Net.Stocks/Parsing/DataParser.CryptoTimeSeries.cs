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
        public TimeSeriesData ParseCryptoTimeSeries(JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var properties = jObject.Children().Select(ch => (JProperty)ch).ToArray();

                var metadataJson = properties.FirstOrDefault(p => p.Name == StockMetaDataJsonToken.MetaDataHeader);
                var timeSeriesJson =
                    properties.FirstOrDefault(p => p.Name.Contains(StockTimeSeriesJsonToken.TimeSeriesHeader));

                if (metadataJson == null || timeSeriesJson == null)
                    throw new TimeSeriesParsingException("Unable to parse time-series json");

                var result = new TimeSeriesData();

                EnrichWithCryptoMetadata(metadataJson, result);
                result.DataPoints = GetCryptoDataPoints(timeSeriesJson);

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


        private void EnrichWithCryptoMetadata([NotNull] JProperty metadataJson, [NotNull] TimeSeriesData timeSeriesData)
        {
            var metadatas = metadataJson.Children().Single();

            foreach (var metadataItem in metadatas)
            {
                var metadataProperty = (JProperty)metadataItem;
                var metadataItemName = metadataProperty.Name;
                var metadataItemValue = metadataProperty.Value.ToString();

                if (metadataItemName.Contains(CryptoMetaDataJsonToken.InformationToken))
                {
                    timeSeriesData.Type = GetTimeSeriesType(metadataItemValue);
                    timeSeriesData.IsAdjusted = IsAdjusted(metadataItemValue);
                }
                else if (metadataItemName.Contains(CryptoMetaDataJsonToken.RefreshTimeToken))
                {
                    var refreshTime = metadataItemValue.Replace("(end of day)", string.Empty).Trim().ParseDateTime();
                    timeSeriesData.LastRefreshed = DateTime.SpecifyKind(refreshTime, DateTimeKind.Local);
                }
                else if (metadataItemName.Contains(CryptoMetaDataJsonToken.SymbolToken))
                {
                    timeSeriesData.Symbol = metadataItemValue;
                }
                else if (metadataItemName.Contains(CryptoMetaDataJsonToken.MarketCode))
                {
                    timeSeriesData.Market = metadataItemValue;
                }
            }
        }

        private ICollection<DataPoint> GetCryptoDataPoints([NotNull] JProperty timeSeriesJson)
        {
            var result = new List<DataPoint>();

            var timeseriesContent = timeSeriesJson.Children().Single();
            var contentDict = new Dictionary<string, string>();
            foreach (var dataPointJson in timeseriesContent)
            {
                var dataPointJsonProperty = dataPointJson as JProperty;
                if (dataPointJsonProperty == null)
                    throw new TimeSeriesParsingException("Unable to parse time-series");

                contentDict.Add(TimeStampKey, dataPointJsonProperty.Name);

                var dataPointContent = dataPointJsonProperty.Single();
                foreach (var field in dataPointContent)
                {
                    var property = (JProperty)field;
                    contentDict.Add(property.Name, property.Value.ToString());
                }

                var dataPoint = ComposeCryptoDataPoint(contentDict);

                result.Add(dataPoint);
                contentDict.Clear();
            }

            return result;
        }

        private DataPoint ComposeCryptoDataPoint(Dictionary<string, string> dataPointContent)
        {

            var dataPoint = new DataPoint();

            dataPoint.Time = dataPointContent[TimeStampKey].ParseDateTime();
            dataPoint.OpeningPrice = dataPointContent[CryptoTimeSeriesJsonToken.OpeningPriceToken].ParseDecimal();
            dataPoint.HighestPrice = dataPointContent[CryptoTimeSeriesJsonToken.HighestPriceToken].ParseDecimal();
            dataPoint.LowestPrice = dataPointContent[CryptoTimeSeriesJsonToken.LowestPriceToken].ParseDecimal();
            dataPoint.ClosingPrice = dataPointContent[CryptoTimeSeriesJsonToken.ClosingPriceToken].ParseDecimal();

            dataPoint.Volume = dataPointContent[CryptoTimeSeriesJsonToken.VolumeToken].ParseDecimal();
            dataPoint.MarketCapUsd = dataPointContent[CryptoTimeSeriesJsonToken.MarketCapUsdToken].ParseDecimal();

            return dataPoint;
        }
    }
}
