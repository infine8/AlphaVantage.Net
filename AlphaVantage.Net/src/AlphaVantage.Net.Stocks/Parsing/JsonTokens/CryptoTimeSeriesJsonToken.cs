namespace AlphaVantage.Net.Stocks.Parsing.JsonTokens
{
    public static class CryptoTimeSeriesJsonToken
    {
        public const string OpeningPriceToken = "1. open";
        public const string HighestPriceToken = "2. high";
        public const string LowestPriceToken = "3. low";
        public const string ClosingPriceToken = "4. close";

        public const string VolumeToken = "5. volume";

        public const string MarketCapUsdToken = "6. market cap (USD)";

    }
}
