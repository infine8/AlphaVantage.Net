using System;

namespace AlphaVantage.Net.Stocks.Listings
{
    /// <summary>
    /// One row of the LISTING_STATUS CSV response.
    /// </summary>
    public class ListingStatusEntry
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public ListingAssetType AssetType { get; set; }
        public DateTime? IpoDate { get; set; }
        public DateTime? DelistingDate { get; set; }
        public ListingStatus Status { get; set; }
    }
}
