namespace AlphaVantage.Net.Stocks.Parsing.JsonTokens
{
    public static class CryptoTimeSeriesJsonToken
    {
        public const string OpeningPriceToken = "1b. open (USD)";
        public const string HighestPriceToken = "2b. high (USD)";
        public const string LowestPriceToken = "3b. low (USD)";
        public const string ClosingPriceToken = "4b. close (USD)";

        public const string VolumeToken = "5. volume";

        public const string MarketCapUsdToken = "6. market cap (USD)";

    }
}
