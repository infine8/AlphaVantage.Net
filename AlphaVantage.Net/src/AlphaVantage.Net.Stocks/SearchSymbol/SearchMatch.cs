using System;

namespace AlphaVantage.Net.Stocks.SearchSymbol
{
    public class SearchMatch
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Region { get; set; }
        public TimeSpan MarketOpenTime { get; set; }
        public TimeSpan MarketCloseTime { get; set; }
        public string Timezone { get; set; }
        public string Currency { get; set; }
        public decimal MatchScore { get; set; }
    }
}
