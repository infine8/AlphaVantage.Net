using System;
using System.ComponentModel;

namespace AlphaVantage.Net.Stocks.Utils
{
    public static class Extensions
    {
		public static string GetEnumDescription(this Enum e)
        {
            var descriptionAttribute = e.GetType().GetMember(e.ToString())[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0]
                as DescriptionAttribute;

            return descriptionAttribute.Description;
        }
	}
}
