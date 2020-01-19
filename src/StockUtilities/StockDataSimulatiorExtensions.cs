using System;
using System.Collections.Generic;
using System.Text;

namespace StockUtilities
{
    /// <summary>
    /// Provide Methods for encapsulating the update logic. In a complex scenario a builder patter would otherwise be used
    /// with interfaces to signal builder stages. This method doesn't pollute with main class with overloads and allows
    /// you to have a stage where it's possible to sanitize inputs instead of putting that logic into the main class.
    /// </summary>
    public static class StockDataSimulatiorExtensions
    {
        public static void AddSymbol( 
            this StockDataSimulator simulator, 
            string name,
            string symbol,
            decimal priceLow,
            decimal priceHigh
        )
        {
            var setting = new SimulationItemSetting( )
            {
                Name = name,
                Symbol = symbol,
                PriceHigh = priceHigh,
                PriceLow = priceLow,
                ReportIntervalStart = 100,
                ReportIntevalEnd = 2000,
                RateOfChange = 0.3m,
                LastValue = priceHigh - ( ( priceHigh - priceLow ) / 2 )
            };

            simulator.AddSymbol( setting );
        }
    }
}
