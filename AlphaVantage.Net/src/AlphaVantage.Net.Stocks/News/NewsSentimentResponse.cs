using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlphaVantage.Net.Stocks.News
{
    /// <summary>
    /// Top-level response of the NEWS_SENTIMENT endpoint.
    /// </summary>
    public class NewsSentimentResponse
    {
        [JsonProperty("items")]
        public string Items { get; set; }

        [JsonProperty("sentiment_score_definition")]
        public string SentimentScoreDefinition { get; set; }

        [JsonProperty("relevance_score_definition")]
        public string RelevanceScoreDefinition { get; set; }

        [JsonProperty("feed")]
        public List<NewsFeedItem> Feed { get; set; }

        /// <summary>
        /// Throttle / rate-limit message returned by the API in lieu of a payload.
        /// </summary>
        [JsonProperty("Note")]
        public string Note { get; set; }

        /// <summary>
        /// "Information" message (e.g. premium-tier required, daily quota exceeded).
        /// </summary>
        [JsonProperty("Information")]
        public string Information { get; set; }
    }
}
