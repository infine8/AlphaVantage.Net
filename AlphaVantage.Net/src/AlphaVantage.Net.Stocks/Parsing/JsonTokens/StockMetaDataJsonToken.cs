﻿namespace AlphaVantage.Net.Stocks.Parsing.JsonTokens
{
    internal static class StockMetaDataJsonToken
    {
        public const string MetaDataHeader = "Meta Data";

        public const string InformationToken = "1. Information";
        public const string SymbolToken = "2. Symbol";
        public const string RefreshTimeToken = "3. Last Refreshed";
        public const string TimezoneToken = "6. Time Zone";
    }
}