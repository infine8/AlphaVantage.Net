using Newtonsoft.Json;

namespace AlphaVantage.Net.Stocks.Fundamentals
{
    /// <summary>
    /// Response of the OVERVIEW endpoint — company fundamentals snapshot.
    /// All numeric values are returned as strings by the API.
    /// </summary>
    public class CompanyOverview
    {
        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("AssetType")]
        public string AssetType { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("CIK")]
        public string Cik { get; set; }

        [JsonProperty("Exchange")]
        public string Exchange { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Sector")]
        public string Sector { get; set; }

        [JsonProperty("Industry")]
        public string Industry { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("FiscalYearEnd")]
        public string FiscalYearEnd { get; set; }

        [JsonProperty("LatestQuarter")]
        public string LatestQuarter { get; set; }

        [JsonProperty("MarketCapitalization")]
        public string MarketCapitalization { get; set; }

        [JsonProperty("EBITDA")]
        public string Ebitda { get; set; }

        [JsonProperty("PERatio")]
        public string PeRatio { get; set; }

        [JsonProperty("PEGRatio")]
        public string PegRatio { get; set; }

        [JsonProperty("BookValue")]
        public string BookValue { get; set; }

        [JsonProperty("DividendPerShare")]
        public string DividendPerShare { get; set; }

        [JsonProperty("DividendYield")]
        public string DividendYield { get; set; }

        [JsonProperty("EPS")]
        public string Eps { get; set; }

        [JsonProperty("RevenuePerShareTTM")]
        public string RevenuePerShareTtm { get; set; }

        [JsonProperty("ProfitMargin")]
        public string ProfitMargin { get; set; }

        [JsonProperty("OperatingMarginTTM")]
        public string OperatingMarginTtm { get; set; }

        [JsonProperty("ReturnOnAssetsTTM")]
        public string ReturnOnAssetsTtm { get; set; }

        [JsonProperty("ReturnOnEquityTTM")]
        public string ReturnOnEquityTtm { get; set; }

        [JsonProperty("RevenueTTM")]
        public string RevenueTtm { get; set; }

        [JsonProperty("GrossProfitTTM")]
        public string GrossProfitTtm { get; set; }

        [JsonProperty("DilutedEPSTTM")]
        public string DilutedEpsTtm { get; set; }

        [JsonProperty("QuarterlyEarningsGrowthYOY")]
        public string QuarterlyEarningsGrowthYoY { get; set; }

        [JsonProperty("QuarterlyRevenueGrowthYOY")]
        public string QuarterlyRevenueGrowthYoY { get; set; }

        [JsonProperty("AnalystTargetPrice")]
        public string AnalystTargetPrice { get; set; }

        [JsonProperty("TrailingPE")]
        public string TrailingPe { get; set; }

        [JsonProperty("ForwardPE")]
        public string ForwardPe { get; set; }

        [JsonProperty("PriceToSalesRatioTTM")]
        public string PriceToSalesRatioTtm { get; set; }

        [JsonProperty("PriceToBookRatio")]
        public string PriceToBookRatio { get; set; }

        [JsonProperty("EVToRevenue")]
        public string EvToRevenue { get; set; }

        [JsonProperty("EVToEBITDA")]
        public string EvToEbitda { get; set; }

        [JsonProperty("Beta")]
        public string Beta { get; set; }

        [JsonProperty("52WeekHigh")]
        public string FiftyTwoWeekHigh { get; set; }

        [JsonProperty("52WeekLow")]
        public string FiftyTwoWeekLow { get; set; }

        [JsonProperty("50DayMovingAverage")]
        public string FiftyDayMovingAverage { get; set; }

        [JsonProperty("200DayMovingAverage")]
        public string TwoHundredDayMovingAverage { get; set; }

        [JsonProperty("SharesOutstanding")]
        public string SharesOutstanding { get; set; }

        [JsonProperty("DividendDate")]
        public string DividendDate { get; set; }

        [JsonProperty("ExDividendDate")]
        public string ExDividendDate { get; set; }
    }
}
