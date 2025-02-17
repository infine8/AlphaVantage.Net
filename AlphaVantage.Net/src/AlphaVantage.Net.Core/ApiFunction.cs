﻿// ReSharper disable InconsistentNaming
namespace AlphaVantage.Net.Core
{
    /// <summary>
    /// Possible functions of Alpha Vantage API
    /// </summary>
    public enum ApiFunction
    {
        // Stock Time Series Data
        TIME_SERIES_INTRADAY,
        TIME_SERIES_DAILY,
        TIME_SERIES_DAILY_ADJUSTED,
        TIME_SERIES_WEEKLY,
        TIME_SERIES_WEEKLY_ADJUSTED,
        TIME_SERIES_MONTHLY,
        TIME_SERIES_MONTHLY_ADJUSTED,
        BATCH_STOCK_QUOTES,

        // Foreign Exchange (FX)
        FX_DAILY,
        
        // Digital & Crypto Currencies
        DIGITAL_CURRENCY_INTRADAY,
        DIGITAL_CURRENCY_DAILY,
        DIGITAL_CURRENCY_WEEKLY,
        DIGITAL_CURRENCY_MONTHLY,

        // Economics Indicators
        REAL_GDP,
        REAL_GDP_PER_CAPITA,
        TREASURY_YIELD,
        FEDERAL_FUNDS_RATE,
        CPI,
        INFLATION,
        RETAIL_SALES,
        DURABLES,
        UNEMPLOYMENT,
        NONFARM_PAYROLL,


        // Stock Technical Indicators
        SMA,
        EMA,
        WMA,
        DEMA,
        TEMA,
        TRIMA,
        KAMA,
        MAMA,
        T3,
        MACD,
        MACDEXT,
        STOCH,
        STOCHF,
        RSI,
        STOCHRSI,
        WILLR,
        ADX,
        ADXR,
        APO,
        PPO,
        MOM,
        BOP,
        CCI,
        CMO,
        ROC,
        ROCR,
        AROON,
        AROONOSC,
        MFI,
        TRIX,
        ULTOSC,
        DX,
        MINUS_DI,
        PLUS_DI,
        MINUS_DM,
        PLUS_DM,
        BBANDS,
        MIDPOINT,
        MIDPRICE,
        SAR,
        TRANGE,
        ATR,
        NATR,
        AD,
        ADOSC,
        OBV,
        HT_TRENDLINE,
        HT_SINE,
        HT_TRENDMODE,
        HT_DCPERIOD,
        HT_DCPHASE,
        HT_PHASOR,
        
        // Sector Performances
        SECTOR,

        SYMBOL_SEARCH
    }
}