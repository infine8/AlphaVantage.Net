using System.ComponentModel;

namespace AlphaVantage.Net.Stocks.Utils
{
    public enum TreasureYieldMaturity
    {
        [Description("30year")] Year30,
        [Description("10year")] Year10,
        [Description("7year")] Year7,
        [Description("5year")] Year5,
        [Description("2year")] Year2,
        [Description("3month")] Month3,
    }
}