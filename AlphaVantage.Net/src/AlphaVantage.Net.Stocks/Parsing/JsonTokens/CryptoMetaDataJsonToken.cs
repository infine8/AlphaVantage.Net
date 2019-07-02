using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaVantage.Net.Stocks.Parsing.JsonTokens
{
    public static class CryptoMetaDataJsonToken
    {
        public const string MetaDataHeader = "Meta Data";

        public const string InformationToken = "1. Information";
        public const string SymbolToken = "2. Digital Currency Code";
        public const string Name = "3. Digital Currency Name";
        public const string MarketCode = "4. Market Code";
        public const string RefreshTimeToken = "6. Last Refreshed";
    }
}
