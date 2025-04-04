using System.IO;
using AlphaVantage.Net.Stocks.Parsing;
using AlphaVantage.Net.Stocks.Parsing.Exceptions;
using AlphaVantage.Net.Stocks.TimeSeries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AlphaVantage.Net.Stocks.Tests.StockDataParserTests
{
    public class TimeSeriesParserTests
    {
        [Fact]
        public void Intraday_ParsingTest()
        {
            var json = File.ReadAllText("Data/intraday.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Intraday, result.Type);
            Assert.False(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(DataPoint), resultDataPoint);
                Assert.IsNotType(typeof(AdjustedDataPoint), resultDataPoint);
            }
        }
        
        [Fact]
        public void DailyNotAdjusted_ParsingTest()
        {
            var json = File.ReadAllText("Data/daily-not-adjusted.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Daily, result.Type);
            Assert.False(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(DataPoint), resultDataPoint);
                Assert.IsNotType(typeof(AdjustedDataPoint), resultDataPoint);
            }
        }
        
        [Fact]
        public void DailyAdjusted_ParsingTest()
        {
            var json = File.ReadAllText("Data/daily-adjusted.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Daily, result.Type);
            Assert.True(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(AdjustedDataPoint), resultDataPoint);
                Assert.NotNull((resultDataPoint as AdjustedDataPoint)?.SplitCoefficient);
            }
        }
        
        [Fact]
        public void WeeklyNotAdjusted_ParsingTest()
        {
            var json = File.ReadAllText("Data/weekly-not-adjusted.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Weekly, result.Type);
            Assert.False(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(DataPoint), resultDataPoint);
                Assert.IsNotType(typeof(AdjustedDataPoint), resultDataPoint);
            }
        }
        
        [Fact]
        public void WeeklyAdjusted_ParsingTest()
        {
            var json = File.ReadAllText("Data/weekly-adjusted.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Weekly, result.Type);
            Assert.True(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(AdjustedDataPoint), resultDataPoint);
            }
        }
        
        [Fact]
        public void MonthlyNotAdjusted_ParsingTest()
        {
            var json = File.ReadAllText("Data/monthly-not-adjusted.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Monthly, result.Type);
            Assert.False(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(DataPoint), resultDataPoint);
                Assert.IsNotType(typeof(AdjustedDataPoint), resultDataPoint);
            }
        }
        
        [Fact]
        public void MonthlyAdjusted_ParsingTest()
        {
            var json = File.ReadAllText("Data/monthly-adjusted.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            var result = parser.ParseStockTimeSeries(jObject);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Monthly, result.Type);
            Assert.True(result.IsAdjusted);
            Assert.Equal(3, result.DataPoints.Count);
            foreach (var resultDataPoint in result.DataPoints)
            {
                Assert.IsType(typeof(AdjustedDataPoint), resultDataPoint);
            }
        }
        
        [Fact]
        public void ParsingError_Test()
        {
            var json = File.ReadAllText("Data/bad-data.json");
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            var parser = new DataParser();
            Assert.Throws(typeof(TimeSeriesParsingException), () => parser.ParseStockTimeSeries(jObject));
        }
    }
}