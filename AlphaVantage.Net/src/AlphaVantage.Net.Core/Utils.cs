using System;
using System.Globalization;

namespace AlphaVantage.Net.Core
{
    public static class Utils
    {
        public static decimal ParseDecimal(this string str)
        {
            decimal.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out var val);

            return val;
        }

        public static long ParseLong(this string str)
        {
            long.TryParse(str, NumberStyles.Number, CultureInfo.InvariantCulture, out var val);

            return val;
        }

        public static DateTime ParseDateTime(this string str)
        {
            DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out var val);

            return val;
        }

        public static DateTime ConvetToUtc(this DateTime dt, string timezone)
        {
            if (timezone.Equals("US/Eastern", StringComparison.OrdinalIgnoreCase))
            {
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                return dt.Add(-easternZone.GetUtcOffset(dt));
            }

            return dt;
        }

        public static TimeSpan ParseTimeSpan(this string str)
        {
            TimeSpan.TryParse(str, out var val);

            return val;
        }
    }
}
