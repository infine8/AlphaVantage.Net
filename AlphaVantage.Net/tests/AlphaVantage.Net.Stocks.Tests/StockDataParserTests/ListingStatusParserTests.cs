using System;
using System.IO;
using System.Linq;
using AlphaVantage.Net.Stocks.Listings;
using Xunit;

namespace AlphaVantage.Net.Stocks.Tests.StockDataParserTests
{
    public class ListingStatusParserTests
    {
        [Fact]
        public void ListingStatus_ParsesAllRows()
        {
            var csv = File.ReadAllText("Data/listing-status.csv");
            var parser = new ListingStatusCsvParser();

            var rows = parser.Parse(csv);

            Assert.Equal(5, rows.Count);
        }

        [Fact]
        public void ListingStatus_MapsStockAndEtfCaseInsensitively()
        {
            var rows = new ListingStatusCsvParser().Parse(File.ReadAllText("Data/listing-status.csv")).ToList();

            var agilent = rows.Single(r => r.Symbol == "A");
            var goldEtf = rows.Single(r => r.Symbol == "AAAU");

            Assert.Equal(ListingAssetType.Stock, agilent.AssetType);
            Assert.Equal(ListingAssetType.Etf,   goldEtf.AssetType);
        }

        [Fact]
        public void ListingStatus_TrimsTrailingWhitespaceInName()
        {
            var rows = new ListingStatusCsvParser().Parse(File.ReadAllText("Data/listing-status.csv")).ToList();

            var adr = rows.Single(r => r.Symbol == "AADR");

            Assert.Equal("ADVISORSHARES DORSEY WRIGHT ADR ETF", adr.Name);
        }

        [Fact]
        public void ListingStatus_TreatsLiteralNullAsMissingDate()
        {
            var rows = new ListingStatusCsvParser().Parse(File.ReadAllText("Data/listing-status.csv")).ToList();

            var agilent = rows.Single(r => r.Symbol == "A");

            Assert.Null(agilent.DelistingDate);
            Assert.Equal(new DateTime(1999, 11, 18), agilent.IpoDate);
        }

        [Fact]
        public void ListingStatus_ParsesDelistedRowWithConcreteDate()
        {
            var rows = new ListingStatusCsvParser().Parse(File.ReadAllText("Data/listing-status.csv")).ToList();

            var lehman = rows.Single(r => r.Symbol == "LEHMQ");

            Assert.Equal(ListingStatus.Delisted, lehman.Status);
            Assert.Equal(new DateTime(2008, 9, 15), lehman.DelistingDate);
            Assert.Equal("NYSE", lehman.Exchange);
        }

        [Fact]
        public void ListingStatus_EmptyInputReturnsEmptyCollection()
        {
            var rows = new ListingStatusCsvParser().Parse("");

            Assert.Empty(rows);
        }
    }
}
