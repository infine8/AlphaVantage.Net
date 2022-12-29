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

        public async Task<ICollection<DateValue>> RequestRealGdpTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
                {"interval", "quarterly"}
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.REAL_GDP, query);
        }

        public async Task<ICollection<DateValue>> RequestRealGdpPerCapitaTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.REAL_GDP_PER_CAPITA, query);
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

        public async Task<ICollection<DateValue>> RequestInterestRateTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
                {"interval", "daily"}
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.FEDERAL_FUNDS_RATE, query);
        }

        public async Task<ICollection<DateValue>> RequestCpiTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
                {"interval", "monthly"}
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.CPI, query);
        }

        public async Task<ICollection<DateValue>> RequestInflationTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.INFLATION, query);
        }

        public async Task<ICollection<DateValue>> RequestRetailSalesTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.RETAIL_SALES, query);
        }

        public async Task<ICollection<DateValue>> RequestDurablesTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.DURABLES, query);
        }

        public async Task<ICollection<DateValue>> RequestUnemploymentTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.UNEMPLOYMENT, query);
        }

        public async Task<ICollection<DateValue>> RequestNonfarmPayrollTimeSeriesAsync()
        {
            var query = new Dictionary<string, string>
            {
            };

            return await RequestTimeSeriesDataAsync(ApiFunction.NONFARM_PAYROLL, query);
        }

        private async Task<ICollection<DateValue>> RequestTimeSeriesDataAsync(ApiFunction function, Dictionary<string, string> query)
        {
            var jObject = await _coreClient.RequestApiAsync(_apiKey, function, query);
            var timeSeries = _parser.ParseEconomicsIndicatorTimeSeries(jObject);

            return timeSeries;
        }
    }
}
