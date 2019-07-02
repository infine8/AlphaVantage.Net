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
    internal partial class DataParser
    {
        public ICollection<SearchMatch> ParseSearchMatches(JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var properties = jObject.Children().Select(ch => (JProperty)ch).ToArray();
                var searchSymbolJson = properties.FirstOrDefault(p => p.Name == SearchSymbolJsonToken.SearchSymbolHeader);
                if (searchSymbolJson == null) throw new TimeSeriesParsingException("Unable to parse search symbol result.");

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
            catch (TimeSeriesParsingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TimeSeriesParsingException("Unable to parse data. See the inner exception for details", ex);
            }
        }

        private SearchMatch ComposeSerachMatch(IDictionary<string, string> searchContent)
        {
            var result = new SearchMatch
            {
                Symbol = searchContent[SearchSymbolJsonToken.Symbol],
                Name = searchContent[SearchSymbolJsonToken.Name],
                Type = searchContent[SearchSymbolJsonToken.Type],
                Region = searchContent[SearchSymbolJsonToken.Region],
                MarketOpenTime = searchContent[SearchSymbolJsonToken.MarketOpenTime].ParseTimeSpan(),
                MarketCloseTime = searchContent[SearchSymbolJsonToken.MarketCloseTime].ParseTimeSpan(),
                Timezone = searchContent[SearchSymbolJsonToken.Timezone],
                Currency = searchContent[SearchSymbolJsonToken.Currency],
                MatchScore = searchContent[SearchSymbolJsonToken.MatchScore].ParseDecimal(),
            };

            return result;
        }
    }
}
