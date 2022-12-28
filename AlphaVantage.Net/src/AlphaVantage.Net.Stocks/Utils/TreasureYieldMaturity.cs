using System.ComponentModel;

namespace AlphaVantage.Net.Stocks.Utils
{
    public enum TreasureYieldMaturity
    {
        [Description("30year")] US30Y,
        [Description("10year")] US10Y,
        [Description("7year")] US7Y,
        [Description("5year")] US5Y,
        [Description("2year")] US2Y,
        [Description("3month")] US3M,
    }
}