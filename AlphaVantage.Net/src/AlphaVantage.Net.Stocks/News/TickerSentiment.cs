using Newtonsoft.Json;

namespace AlphaVantage.Net.Stocks.News
{
    /// <summary>
    /// Per-ticker sentiment attribution within a single news feed entry.
    /// </summary>
    public class TickerSentiment
    {
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("relevance_score")]
        public string RelevanceScore { get; set; }

        [JsonProperty("ticker_sentiment_score")]
        public string SentimentScore { get; set; }

        [JsonProperty("ticker_sentiment_label")]
        public string SentimentLabel { get; set; }
    }
}
