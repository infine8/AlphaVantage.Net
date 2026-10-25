using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace AlphaVantage.Net.Stocks.Listings
{
    public class ListingStatusCsvParser
    {
        public ICollection<ListingStatusEntry> Parse(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return Array.Empty<ListingStatusEntry>();

            using var reader = new StringReader(csv);
            using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null,
                BadDataFound = null
            });

            csvReader.Context.RegisterClassMap<ListingStatusEntryMap>();
            return csvReader.GetRecords<ListingStatusEntry>().ToList();
        }
    }

    internal sealed class ListingStatusEntryMap : ClassMap<ListingStatusEntry>
    {
        public ListingStatusEntryMap()
        {
            Map(m => m.Symbol).Name("symbol");
            Map(m => m.Name).Name("name");
            Map(m => m.Exchange).Name("exchange");
            Map(m => m.AssetType).Name("assetType").TypeConverter<EnumIgnoreCaseConverter<ListingAssetType>>();
            Map(m => m.IpoDate).Name("ipoDate").TypeConverter<NullableIsoDateConverter>();
            Map(m => m.DelistingDate).Name("delistingDate").TypeConverter<NullableIsoDateConverter>();
            Map(m => m.Status).Name("status").TypeConverter<EnumIgnoreCaseConverter<ListingStatus>>();
        }
    }

    /// <summary>
    /// LISTING_STATUS emits the literal string "null" (not an empty field) when a date is missing —
    /// map that to <c>null</c> and otherwise parse yyyy-MM-dd.
    /// </summary>
    internal sealed class NullableIsoDateConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text) || string.Equals(text.Trim(), "null", StringComparison.OrdinalIgnoreCase))
                return null;

            return DateTime.ParseExact(text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }

    /// <summary>
    /// AlphaVantage returns "ETF" / "Stock" / "Active" / "Delisted" — parse into the corresponding
    /// enum member ignoring case, since our enum uses PascalCase.
    /// </summary>
    internal sealed class EnumIgnoreCaseConverter<TEnum> : DefaultTypeConverter where TEnum : struct, Enum
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new TypeConverterException(this, memberMapData, text, row.Context,
                    $"Cannot convert empty value to {typeof(TEnum).Name}");

            if (!Enum.TryParse<TEnum>(text.Trim(), ignoreCase: true, out var value))
                throw new TypeConverterException(this, memberMapData, text, row.Context,
                    $"Unknown {typeof(TEnum).Name} value: '{text}'");

            return value;
        }
    }
}
