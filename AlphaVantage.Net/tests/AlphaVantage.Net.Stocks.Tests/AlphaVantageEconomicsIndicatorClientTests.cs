using System;
using System.Threading;
using System.Threading.Tasks;
using AlphaVantage.Net.Stocks.Utils;
using Xunit;

namespace AlphaVantage.Net.Stocks.Tests
{
    public class AlphaVantageEconomicsIndicatorClientTests
    {
        private const string Symbol = "LFAC";
        private const string ApiKey = "MSFSF1JFG104HAPL";

        public AlphaVantageEconomicsIndicatorClientTests()
        {
            Thread.Sleep(TimeSpan.FromSeconds(10)); // to avoid api rejection
        }


        [Fact]
        public async Task RequestDailyTimeSeriesAsync_Test()
        {
            var client = new AlphaVantageEconomicsIndicatorClient(ApiKey);

            var result = await client.RequestTreasureYieldDailyTimeSeriesAsync(TreasureYieldMaturity.Month3);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
