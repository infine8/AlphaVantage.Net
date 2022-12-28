using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AlphaVantage.Net.Core;
using AlphaVantage.Net.Stocks.Parsing;
using AlphaVantage.Net.Stocks.TimeSeries;
using AlphaVantage.Net.Stocks.Utils;

namespace AlphaVantage.Net.Stocks
{
    public class AlphaVantageEconomicsIndicatorClient
    {
        private readonly string _apiKey;
        private readonly AlphaVantageCoreClient _coreClient;
        private readonly DataParser _parser;

        public AlphaVantageEconomicsIndicatorClient(string apiKey, TimeSpan? requestTimeout = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));

            _apiKey = apiKey;
            _coreClient = new AlphaVantageCoreClient(timeout: requestTimeout);
            _parser = new DataParser();
        }

        public async Task<ICollection<DateValue>> RequestTreasureYieldDailyTimeSeriesAsync(TreasureYieldMaturity maturity)
        {
            var query = new Dictionary<string, string>
            {
                {"interval", "daily"},
                {"maturity", maturity.GetEnumDescription()}
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.TREASURY_YIELD, query);
        }

        private async Task<ICollection<DateValue>> RequestTimeSeriesDataAsync(ApiFunction function, Dictionary<string, string> query)
        {
            var jObject = await _coreClient.RequestApiAsync(_apiKey, function, query);
            var timeSeries = _parser.ParseEconomicsIndicatorTimeSeries(jObject);

            return timeSeries;
        }
    }
}
