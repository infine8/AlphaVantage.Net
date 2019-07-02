using System;

namespace AlphaVantage.Net.Stocks.TimeSeries
{
    /// <summary>
    /// Represent single adjusted element of time series
    /// </summary>
    public sealed class AdjustedDataPoint : DataPoint
    {
        public decimal AdjustedClosingPrice {get; set;}
        
        public decimal DividendAmount {get; set;}
        
        public decimal? SplitCoefficient { get; set; }
    }
}