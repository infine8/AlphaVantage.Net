using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlphaVantage.Net.Core;
using AlphaVantage.Net.Stocks.BatchQuotes;
using AlphaVantage.Net.Stocks.Parsing.Exceptions;
using AlphaVantage.Net.Stocks.Parsing.JsonTokens;
using AlphaVantage.Net.Stocks.TimeSeries;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace AlphaVantage.Net.Stocks.Parsing
{
    internal partial class DataParser
    {
        private const string TimeStampKey = "timestamp";
        
        public TimeSeriesData ParseStockTimeSeries(JObject jObject)
        {
            if(jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var properties = jObject.Children().Select(ch => (JProperty) ch).ToArray();

                var metadataJson = properties.FirstOrDefault(p => p.Name == StockMetaDataJsonToken.MetaDataHeader);
                var timeSeriesJson =
                    properties.FirstOrDefault(p => p.Name.Contains(StockTimeSeriesJsonToken.TimeSeriesHeader));

                if (metadataJson == null || timeSeriesJson == null)
                    throw new TimeSeriesParsingException("Unable to parse time-series json");

                var result = new TimeSeriesData();

                EnrichWithStockMetadata(metadataJson, result);
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

        private void EnrichWithStockMetadata([NotNull] JProperty metadataJson, [NotNull] TimeSeriesData timeSeriesData)
        {
            var metadatas = metadataJson.Children().Single();

            foreach (var metadataItem in metadatas)
            {
                var metadataProperty = (JProperty) metadataItem;
                var metadataItemName = metadataProperty.Name;
                var metadataItemValue = metadataProperty.Value.ToString();
                
                if (metadataItemName.Contains(StockMetaDataJsonToken.InformationToken))
                {
                    timeSeriesData.Type = GetTimeSeriesType(metadataItemValue);
                    timeSeriesData.IsAdjusted = IsAdjusted(metadataItemValue);
                }
                else if(metadataItemName.Contains(StockMetaDataJsonToken.RefreshTimeToken))
                {
                    var refreshTime = metadataItemValue.ParseDateTime();
                    timeSeriesData.LastRefreshed = DateTime.SpecifyKind(refreshTime, DateTimeKind.Local);
                }
                else if (metadataItemName.Contains(StockMetaDataJsonToken.SymbolToken))
                {
                    timeSeriesData.Symbol = metadataItemValue;
                }
            }
        }

        private TimeSeriesType GetTimeSeriesType(string metadataValue)
        {
            if (metadataValue.Contains(StockTimeSeriesJsonToken.IntradayTimeSeriesTypeToken))
                return TimeSeriesType.Intraday;
            if (metadataValue.Contains(StockTimeSeriesJsonToken.DailyTimeSeriesTypeToken))
                return TimeSeriesType.Daily;
            if (metadataValue.Contains(StockTimeSeriesJsonToken.WeeklyTimeSeriesTypeToken))
                return TimeSeriesType.Weekly;
            if (metadataValue.Contains(StockTimeSeriesJsonToken.MonthlyTimeSeriesTypeToken))
                return TimeSeriesType.Monthly;
            
            throw new TimeSeriesParsingException("Unable to determine time-series type");
        }
        
        private bool IsAdjusted(string metadataValue)
        {
            return 
                metadataValue.Equals(StockTimeSeriesJsonToken.AdjustedToken_1) ||
                metadataValue.Contains(StockTimeSeriesJsonToken.AdjustedToken_2);
        }
        
        private ICollection<DataPoint> GetStockDataPoints([NotNull] JProperty timeSeriesJson)
        {
            var result = new List<DataPoint>();
            
            var timeseriesContent = timeSeriesJson.Children().Single();
            var contentDict = new Dictionary<string, string>();
            foreach (var dataPointJson in timeseriesContent)
            {
                var dataPointJsonProperty = dataPointJson as JProperty;
                if(dataPointJsonProperty == null)
                    throw new TimeSeriesParsingException("Unable to parse time-series");
                
                contentDict.Add(TimeStampKey, dataPointJsonProperty.Name);
                
                var dataPointContent = dataPointJsonProperty.Single();
                foreach (var field in dataPointContent)
                {
                    var property = (JProperty) field;
                    contentDict.Add(property.Name, property.Value.ToString());
                }

                var dataPoint = ComposeStockDataPoint(contentDict);
                
                result.Add(dataPoint);
                contentDict.Clear();
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private DataPoint ComposeStockDataPoint(Dictionary<string, string> dataPointContent)
        {
            var isAdjusted = dataPointContent.Count > 6;

            var dataPoint = isAdjusted ? new AdjustedDataPoint() : new DataPoint();

            dataPoint.Time = dataPointContent[TimeStampKey].ParseDateTime();
            dataPoint.OpeningPrice = dataPointContent[StockTimeSeriesJsonToken.OpeningPriceToken].ParseDecimal();
            dataPoint.HighestPrice = dataPointContent[StockTimeSeriesJsonToken.HighestPriceToken].ParseDecimal();
            dataPoint.LowestPrice = dataPointContent[StockTimeSeriesJsonToken.LowestPriceToken].ParseDecimal();
            dataPoint.ClosingPrice = dataPointContent[StockTimeSeriesJsonToken.ClosingPriceToken].ParseDecimal();

            if (isAdjusted)
            {
                var adjustedPoint = dataPoint as AdjustedDataPoint;
                adjustedPoint.Volume = dataPointContent[StockTimeSeriesJsonToken.VolumeAdjustedToken].ParseDecimal();
                adjustedPoint.AdjustedClosingPrice = dataPointContent[StockTimeSeriesJsonToken.AdjustedClosingPriceToken].ParseDecimal();
                adjustedPoint.DividendAmount = dataPointContent[StockTimeSeriesJsonToken.DividendAmountToken].ParseDecimal();
                
                if(dataPointContent.ContainsKey(StockTimeSeriesJsonToken.SplitCoefficientToken))
                    adjustedPoint.SplitCoefficient = dataPointContent[StockTimeSeriesJsonToken.SplitCoefficientToken].ParseDecimal();
            }
            else
            {
                dataPoint.Volume = dataPointContent[StockTimeSeriesJsonToken.VolumeNonAdjustedToken].ParseDecimal();
            }
            
            return dataPoint;
        }
    }
}