using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlphaVantage.Net.Core;
using AlphaVantage.Net.Stocks.BatchQuotes;
using AlphaVantage.Net.Stocks.Parsing.Exceptions;
using AlphaVantage.Net.Stocks.Parsing.JsonTokens;
using AlphaVantage.Net.Stocks.SearchSymbol;
using Newtonsoft.Json.Linq;

namespace AlphaVantage.Net.Stocks.Parsing
{
    internal partial class StockDataParser
    {
        public ICollection<SearchMatch> ParseSearchMatches(JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var properties = jObject.Children().Select(ch => (JProperty)ch).ToArray();
                var searchSymbolJson = properties.FirstOrDefault(p => p.Name == SearchSymbolJsonTokens.SearchSymbolHeader);
                if (searchSymbolJson == null) throw new StocksParsingException("Unable to parse search symbol result.");

                var result = new List<SearchMatch>();
                var contentDict = new Dictionary<string, string>();
                foreach (var quoteJson in searchSymbolJson.Value)
                {
                    var quoteProperties = quoteJson.Children().Select(q => (JProperty)q).ToArray();
                    foreach (var quoteProperty in quoteProperties)
                    {
                        contentDict.Add(quoteProperty.Name, quoteProperty.Value.ToString());
                    }

                    var quote = ComposeSerachMatch(contentDict);
                    contentDict.Clear();

                    result.Add(quote);
                }

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

        private SearchMatch ComposeSerachMatch(IDictionary<string, string> searchContent)
        {
            var result = new SearchMatch
            {
                Symbol = searchContent[SearchSymbolJsonTokens.Symbol],
                Name = searchContent[SearchSymbolJsonTokens.Name],
                Type = searchContent[SearchSymbolJsonTokens.Type],
                Region = searchContent[SearchSymbolJsonTokens.Region],
                MarketOpenTime = searchContent[SearchSymbolJsonTokens.MarketOpenTime].ParseTimeSpan(),
                MarketCloseTime = searchContent[SearchSymbolJsonTokens.MarketCloseTime].ParseTimeSpan(),
                Timezone = searchContent[SearchSymbolJsonTokens.Timezone],
                Currency = searchContent[SearchSymbolJsonTokens.Currency],
                MatchScore = searchContent[SearchSymbolJsonTokens.MatchScore].ParseDecimal(),
            };

            return result;
        }
    }
}
