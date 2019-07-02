using System;
using AlphaVantage.Net.Core.Exceptions;

namespace AlphaVantage.Net.Stocks.Parsing.Exceptions
{
    public class TimeSeriesParsingException : AlphaVantageException
    {
        public TimeSeriesParsingException(string message) : base(message)
        {
        }

        public TimeSeriesParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}