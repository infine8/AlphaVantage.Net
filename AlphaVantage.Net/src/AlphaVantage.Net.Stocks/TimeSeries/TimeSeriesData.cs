using System;
using System.Collections.Generic;

namespace AlphaVantage.Net.Stocks.TimeSeries
{
    public class TimeSeriesData
    {
        public TimeSeriesType Type { get; set; }

        public DateTime LastRefreshed { get; set; }

        public string Symbol { get; set; }

        public string Market { get; set; }

        public bool IsAdjusted { get; set; }

        public ICollection<DataPoint> DataPoints { get; set; }
    }
}
