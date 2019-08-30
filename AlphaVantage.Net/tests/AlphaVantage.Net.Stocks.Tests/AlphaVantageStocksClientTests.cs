using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlphaVantage.Net.Stocks.TimeSeries;
using Xunit;

namespace AlphaVantage.Net.Stocks.Tests
{
    public class AlphaVantageStocksClientTests
    {
        private const string Symbol = "AAPL";
        private const string ApiKey = "MSFSF1JFG104HAPL";

        public AlphaVantageStocksClientTests()
        {
            Thread.Sleep(TimeSpan.FromSeconds(10)); // to avoid api rejection
        }
        
        [Fact]
        public async Task RequestIntradayTimeSeriesAsync_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockIntradayTimeSeriesAsync(Symbol, IntradayInterval.OneMin, TimeSeriesSize.Compact);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Intraday, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => 
                p.GetType() == typeof(DataPoint) &&
                p.GetType() != typeof(AdjustedDataPoint)));
        }
        
        [Fact]
        public async Task RequestDailyTimeSeriesAsync_NotAdjusted_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockDailyTimeSeriesAsync(Symbol, TimeSeriesSize.Compact, adjusted: false);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Daily, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.False(result.IsAdjusted);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => 
                p.GetType() == typeof(DataPoint) &&
                p.GetType() != typeof(AdjustedDataPoint)));
        }
        
        [Fact]
        public async Task RequestDailyTimeSeriesAsync_Adjusted_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockDailyTimeSeriesAsync(Symbol, TimeSeriesSize.Compact, adjusted: true);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Daily, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.True(result.IsAdjusted);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => p is AdjustedDataPoint));
        }
        
        [Fact]
        public async Task RequestWeeklyTimeSeriesAsync_NotAdjusted_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockWeeklyTimeSeriesAsync(Symbol, adjusted: false);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Weekly, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.False(result.IsAdjusted);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => 
                p.GetType() == typeof(DataPoint) &&
                p.GetType() != typeof(AdjustedDataPoint)));
        }
        
        [Fact]
        public async Task RequestWeeklyTimeSeriesAsync_Adjusted_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockWeeklyTimeSeriesAsync(Symbol, adjusted: true);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Weekly, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.True(result.IsAdjusted);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => p is AdjustedDataPoint));
        }
        
        [Fact]
        public async Task RequestMonthlyTimeSeriesAsync_NotAdjusted_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockMonthlyTimeSeriesAsync(Symbol, adjusted: false);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Monthly, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.False(result.IsAdjusted);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => 
                p.GetType() == typeof(DataPoint) &&
                p.GetType() != typeof(AdjustedDataPoint)));
        }
        
        [Fact]
        public async Task RequestMonthlyTimeSeriesAsync_Adjusted_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestStockMonthlyTimeSeriesAsync(Symbol, adjusted: true);
            
            Assert.NotNull(result);
            Assert.Equal(TimeSeriesType.Monthly, result.Type);
            Assert.Equal(Symbol, result.Symbol);
            Assert.True(result.IsAdjusted);
            Assert.NotNull(result.DataPoints);
            Assert.True(result.DataPoints.All(p => p is AdjustedDataPoint));
        }
        
        [Fact]
        public async Task RequestBatchQuotesAsync_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result =
                await client.RequestBatchQuotesAsync(new []{"AAPL", "FB", "MSFT"});
            
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.True(
                result.Any(r => r.Symbol == "MSFT") &&
                result.Any(r => r.Symbol == "FB") && 
                result.Any(r => r.Symbol == "AAPL"));
        }

        [Fact]
        public async Task RequestSymbolSearchAsync_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result = await client.RequestSearchAsync("RJZ");

            Assert.NotNull(result);

            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task RequestCryptoDailyTimeSeriesAsync_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result = await client.RequestCryptoDailyTimeSeriesAsync("BTC");

            Assert.NotNull(result);

            Assert.True(result.DataPoints.Count > 0);
        }

        [Fact]
        public async Task RequestFxDailyTimeSeriesAsync_Test()
        {
            var client = new AlphaVantageStocksClient(ApiKey);

            var result = await client.RequestFxDailyTimeSeriesAsync("USD", "RUB");

            Assert.NotNull(result);

            Assert.True(result.DataPoints.Count > 0);
        }
    }
}