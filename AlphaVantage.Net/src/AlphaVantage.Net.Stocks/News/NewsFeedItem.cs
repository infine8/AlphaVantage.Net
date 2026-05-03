using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaVantage.Net.Stocks.News
{
    /// <summary>
    /// A single news article entry returned by the NEWS_SENTIMENT endpoint.
    /// </summary>
    public class NewsFeedItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("time_published")]
        public string TimePublished { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("source_domain")]
        public string SourceDomain { get; set; }

        [JsonProperty("category_within_source")]
        public string CategoryWithinSource { get; set; }

        [JsonProperty("overall_sentiment_score")]
        public decimal? OverallSentimentScore { get; set; }

        [JsonProperty("overall_sentiment_label")]
        public string OverallSentimentLabel { get; set; }

        [JsonProperty("ticker_sentiment")]
        public List<TickerSentiment> TickerSentiments { get; set; }
    }
}
