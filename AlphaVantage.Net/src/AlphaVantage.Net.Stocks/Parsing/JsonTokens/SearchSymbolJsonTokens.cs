namespace AlphaVantage.Net.Stocks.Parsing.JsonTokens
{
    internal static class SearchSymbolJsonTokens
    {
        public const string SearchSymbolHeader = "bestMatches";

        public const string Symbol = "1. symbol";
        public const string Name = "2. name";
        public const string Type = "3. type";
        public const string Region = "4. region";
        public const string MarketOpenTime = "5. marketOpen";
        public const string MarketCloseTime = "6. marketClose";
        public const string Timezone = "7. timezone";
        public const string Currency = "8. currency";
        public const string MatchScore = "9. matchScore";
    }
}
