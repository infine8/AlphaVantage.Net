using System;

namespace AlphaVantage.Net.Stocks.TimeSeries
{
    /// <summary>
    /// Represent single element of time series
    /// </summary>
    public class DataPoint
    {
        public DateTime Time { get; set; }

        public decimal OpeningPrice { get; set; }

        public decimal ClosingPrice { get; set; }
        public decimal AdjustedClosingPrice { get; set; }
        public decimal SplitCoefficient { get; set; }

        public decimal HighestPrice { get; set; }

        public decimal LowestPrice { get; set; }

        public decimal? Volume { get; set; }

        public decimal? MarketCapUsd { get; set; }
    }
}