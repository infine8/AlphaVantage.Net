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
    internal partial class StockDataParser
    {
        private const string TimeStampKey = "timestamp";
        
        public StockTimeSeries ParseTimeSeries(JObject jObject)
        {
            if(jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var properties = jObject.Children().Select(ch => (JProperty) ch).ToArray();

                var metadataJson = properties.FirstOrDefault(p => p.Name == MetaDataJsonTokens.MetaDataHeader);
                var timeSeriesJson =
                    properties.FirstOrDefault(p => p.Name.Contains(TimeSeriesJsonTokens.TimeSeriesHeader));

                if (metadataJson == null || timeSeriesJson == null)
                    throw new StocksParsingException("Unable to parse time-series json");

                var result = new StockTimeSeries();

                EnrichWithMetadata(metadataJson, result);
                result.DataPoints = GetStockDataPoints(timeSeriesJson);

                return result;
            }
            catch (StocksParsingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StocksParsingException("Unable to parse data. See the inner exception for details", ex);
            }
        }

        private void EnrichWithMetadata([NotNull] JProperty metadataJson, [NotNull] StockTimeSeries timeSeries)
        {
            var metadatas = metadataJson.Children().Single();

            foreach (var metadataItem in metadatas)
            {
                var metadataProperty = (JProperty) metadataItem;
                var metadataItemName = metadataProperty.Name;
                var metadataItemValue = metadataProperty.Value.ToString();
                
                if (metadataItemName.Contains(MetaDataJsonTokens.InformationToken))
                {
                    timeSeries.Type = GetTimeSeriesType(metadataItemValue);
                    timeSeries.IsAdjusted = IsAdjusted(metadataItemValue);
                }
                else if(metadataItemName.Contains(MetaDataJsonTokens.RefreshTimeToken))
                {
                    var refreshTime = DateTime.Parse(metadataItemValue);
                    timeSeries.LastRefreshed = DateTime.SpecifyKind(refreshTime, DateTimeKind.Local);
                }
                else if (metadataItemName.Contains(MetaDataJsonTokens.SymbolToken))
                {
                    timeSeries.Symbol = metadataItemValue;
                }
            }
        }

        private TimeSeriesType GetTimeSeriesType(string metadataValue)
        {
            if (metadataValue.Contains(TimeSeriesJsonTokens.IntradayTimeSeriesTypeToken))
                return TimeSeriesType.Intraday;
            if (metadataValue.Contains(TimeSeriesJsonTokens.DailyTimeSeriesTypeToken))
                return TimeSeriesType.Daily;
            if (metadataValue.Contains(TimeSeriesJsonTokens.WeeklyTimeSeriesTypeToken))
                return TimeSeriesType.Weekly;
            if (metadataValue.Contains(TimeSeriesJsonTokens.MonthlyTimeSeriesTypeToken))
                return TimeSeriesType.Monthly;
            
            throw new StocksParsingException("Unable to determine time-series type");
        }
        
        private bool IsAdjusted(string metadataValue)
        {
            return 
                metadataValue.Equals(TimeSeriesJsonTokens.AdjustedToken_1) ||
                metadataValue.Contains(TimeSeriesJsonTokens.AdjustedToken_2);
        }
        
        private ICollection<StockDataPoint> GetStockDataPoints([NotNull] JProperty timeSeriesJson)
        {
            var result = new List<StockDataPoint>();
            
            var timeseriesContent = timeSeriesJson.Children().Single();
            var contentDict = new Dictionary<string, string>();
            foreach (var dataPointJson in timeseriesContent)
            {
                var dataPointJsonProperty = dataPointJson as JProperty;
                if(dataPointJsonProperty == null)
                    throw new StocksParsingException("Unable to parse time-series");
                
                contentDict.Add(TimeStampKey, dataPointJsonProperty.Name);
                
                var dataPointContent = dataPointJsonProperty.Single();
                foreach (var field in dataPointContent)
                {
                    var property = (JProperty) field;
                    contentDict.Add(property.Name, property.Value.ToString());
                }

                var dataPoint = ComposeDataPoint(contentDict);
                
                result.Add(dataPoint);
                contentDict.Clear();
            }

            return result;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private StockDataPoint ComposeDataPoint(Dictionary<string, string> dataPointContent)
        {
            var isAdjusted = dataPointContent.Count > 6;

            var dataPoint = isAdjusted ? new StockAdjustedDataPoint() : new StockDataPoint();
            
            dataPoint.Time = DateTime.Parse(dataPointContent[TimeStampKey]);
            dataPoint.OpeningPrice = dataPointContent[TimeSeriesJsonTokens.OpeningPriceToken].ParseDecimal();
            dataPoint.HighestPrice = dataPointContent[TimeSeriesJsonTokens.HighestPriceToken].ParseDecimal();
            dataPoint.LowestPrice = dataPointContent[TimeSeriesJsonTokens.LowestPriceToken].ParseDecimal();
            dataPoint.ClosingPrice = dataPointContent[TimeSeriesJsonTokens.ClosingPriceToken].ParseDecimal();

            if (isAdjusted)
            {
                var adjustedPoint = dataPoint as StockAdjustedDataPoint;
                adjustedPoint.Volume = dataPointContent[TimeSeriesJsonTokens.VolumeAdjustedToken].ParseLong();
                adjustedPoint.AdjustedClosingPrice = dataPointContent[TimeSeriesJsonTokens.AdjustedClosingPriceToken].ParseDecimal();
                adjustedPoint.DividendAmount = dataPointContent[TimeSeriesJsonTokens.DividendAmountToken].ParseDecimal();
                
                if(dataPointContent.ContainsKey(TimeSeriesJsonTokens.SplitCoefficientToken))
                    adjustedPoint.SplitCoefficient = dataPointContent[TimeSeriesJsonTokens.SplitCoefficientToken].ParseDecimal();
            }
            else
            {
                dataPoint.Volume = dataPointContent[TimeSeriesJsonTokens.VolumeNonAdjustedToken].ParseLong();
            }
            
            return dataPoint;
        }
    }
}