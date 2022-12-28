using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AlphaVantage.Net.Stocks.Parsing.Exceptions;
using AlphaVantage.Net.Stocks.TimeSeries;
using Newtonsoft.Json.Linq;

namespace AlphaVantage.Net.Stocks.Parsing
{
    internal partial class DataParser
    {
        public ICollection<DateValue> ParseEconomicsIndicatorTimeSeries(JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            try
            {
                var result = new List<DateValue>();

                var properties = jObject.Children().Select(ch => (JProperty)ch).ToArray();

                var dataJson = properties.FirstOrDefault(p => p.Name == "data");
                if (dataJson == null)
                    throw new TimeSeriesParsingException($"Unable to parse time-series json. Response: {jObject}");

                var items = dataJson.Children().Single().ToArray();

                foreach (var item in items)
                {

                    var date = DateTime.Parse(item["date"].ToString());
                    if(!decimal.TryParse(item["value"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value)) continue;

                    result.Add(new DateValue { Date = date, Value = value });
                }

                return result;
            }
            catch (TimeSeriesParsingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TimeSeriesParsingException("Unable to parse data. See the inner exception for details", ex);
            }
        }
    }
}
