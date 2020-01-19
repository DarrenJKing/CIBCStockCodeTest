namespace StockUtilities
{
    /// <summary>
    /// This class takes care of the configuration for each Stock symbol simualted.
    /// </summary>
    internal class SimulationItemSetting
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal PriceLow { get; set; } // decimal overkill but for financial applicaions is pragmatism performance not an issue.

        public decimal PriceHigh { get; set; }

        public int ReportIntervalStart { get; set; }

        public int ReportIntevalEnd { get; set; }

        public decimal RateOfChange { get; set; }

        public decimal LastValue { get; set; }
    }
}
