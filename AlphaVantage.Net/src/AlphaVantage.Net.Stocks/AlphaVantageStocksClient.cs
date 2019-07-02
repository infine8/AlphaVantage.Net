using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaVantage.Net.Core;
using AlphaVantage.Net.Stocks.BatchQuotes;
using AlphaVantage.Net.Stocks.Parsing;
using AlphaVantage.Net.Stocks.SearchSymbol;
using AlphaVantage.Net.Stocks.TimeSeries;
using AlphaVantage.Net.Stocks.Utils;
using AlphaVantage.Net.Stocks.Validation;

namespace AlphaVantage.Net.Stocks
{
    /// <summary>
    /// Client for Alpha Vantage API (stock data only)
    /// </summary>
    public class AlphaVantageStocksClient
    {
        private readonly string _apiKey;
        private readonly AlphaVantageCoreClient _coreClient;
        private readonly DataParser _parser;
        
        public AlphaVantageStocksClient(string apiKey)
        {
            if(string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            
            _apiKey = apiKey;
            _coreClient = new AlphaVantageCoreClient();
            _parser = new DataParser();
        }

        public async Task<TimeSeriesData> RequestStockIntradayTimeSeriesAsync(
            string symbol, 
            IntradayInterval interval = IntradayInterval.SixtyMin, 
            TimeSeriesSize size = TimeSeriesSize.Compact)
        {
            var query = new Dictionary<string, string>()
            {
                {StockApiQueryVars.Symbol, symbol},
                {StockApiQueryVars.IntradayInterval, interval.ConvertToString()},
                {StockApiQueryVars.OutputSize, size.ConvertToString()}
            };
            
            return await RequestTimeSeriesDataAsync(ApiFunction.TIME_SERIES_INTRADAY, query);
        }

        public async Task<TimeSeriesData> RequestStockDailyTimeSeriesAsync(
            string symbol, 
            TimeSeriesSize size = TimeSeriesSize.Compact, 
            bool adjusted = false)
        {
            var query = new Dictionary<string, string>()
            {
                {StockApiQueryVars.Symbol, symbol},
                {StockApiQueryVars.OutputSize, size.ConvertToString()}
            };
            
            var function = adjusted ? 
                ApiFunction.TIME_SERIES_DAILY_ADJUSTED : 
                ApiFunction.TIME_SERIES_DAILY;
            
            return await RequestTimeSeriesDataAsync(function, query);
        }
        
        public async Task<TimeSeriesData> RequestStockWeeklyTimeSeriesAsync(string symbol, bool adjusted = false)
        {
            var query = new Dictionary<string, string>()
            {
                {StockApiQueryVars.Symbol, symbol},
            };
            
            var function = adjusted ? 
                ApiFunction.TIME_SERIES_WEEKLY_ADJUSTED : 
                ApiFunction.TIME_SERIES_WEEKLY;
            
            return await RequestTimeSeriesDataAsync(function, query);
        }

        public async Task<TimeSeriesData> RequestStockMonthlyTimeSeriesAsync(string symbol, bool adjusted = false)
        {
            var query = new Dictionary<string, string>()
            {
                {StockApiQueryVars.Symbol, symbol},
            };
            
            var function = adjusted ? 
                ApiFunction.TIME_SERIES_MONTHLY_ADJUSTED : 
                ApiFunction.TIME_SERIES_MONTHLY;

            return await RequestTimeSeriesDataAsync(function, query);
        }

        public async Task<ICollection<StockQuote>> RequestBatchQuotesAsync(string[] symbols)
        {
            var symbolsString = string.Join(",", symbols);
            
            var query = new Dictionary<string, string>()
            {
                {StockApiQueryVars.BatchSymbols, symbolsString},
            };
                        
            var jObject = await _coreClient.RequestApiAsync(_apiKey, ApiFunction.BATCH_STOCK_QUOTES, query);
            var timeSeries = _parser.ParseStockQuotes(jObject);
            
            return timeSeries;
        }

        private async Task<TimeSeriesData> RequestTimeSeriesDataAsync(ApiFunction function, Dictionary<string, string> query)
        {
            var jObject = await _coreClient.RequestApiAsync(_apiKey, function, query);
            var timeSeries = _parser.ParseStockTimeSeries(jObject);
            
            return timeSeries;
        }


        public async Task<ICollection<SearchMatch>> RequestSearchAsync(string keyword)
        {
            keyword = keyword?.Trim();

            if (string.IsNullOrEmpty(keyword)) return null;

            var jObject = await _coreClient.RequestApiAsync(_apiKey, ApiFunction.SYMBOL_SEARCH, new Dictionary<string, string> { { "keywords", keyword } });

            var searchMatches = _parser.ParseSearchMatches(jObject);

            return searchMatches;
        }

        public async Task<TimeSeriesData> RequestCryptoDailyTimeSeriesAsync(string symbol, string market = "USD")
        {
            symbol = symbol?.Trim().ToUpper();
            market = market?.Trim().ToUpper();

            if (string.IsNullOrEmpty(symbol) || string.IsNullOrEmpty(market)) return null;

            var jObject = await _coreClient.RequestApiAsync(_apiKey, ApiFunction.DIGITAL_CURRENCY_DAILY, new Dictionary<string, string> { { "symbol", symbol }, { "market", market } });
            var timeSeries = _parser.ParseCryptoTimeSeries(jObject);

            return timeSeries;
        }

    }
}