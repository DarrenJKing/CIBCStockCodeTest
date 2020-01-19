using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace StockUtilities.Test
{
    public class StockDataSimulatorTests
    {
        [Fact]
        public void Create_Test( )
        {
            new StockDataSimulator( );
        }

        [Fact]
        public async Task Observe_Stock_Updates( )
        {
            var d = new StockDataSimulator( );
            d.AddSymbol( "Stock1", "SCK1", 240, 270 );
            d.AddSymbol( "Stock2", "SCK2", 180, 210 );
            var obs = d.GetObservable( );
            IList<Tuple<string, string, decimal>> result = await obs.Take( 10 ).ToList( );

            Assert.Equal( expected: 10, result.Count );
            Assert.Contains( result, d => d.Item1 == "Stock1" );
            Assert.Contains( result, d => d.Item1 == "Stock2" );
        }

        [Fact]
        public async Task Observer_Stock_Multiple_Updates( )
        {
            var d = new StockDataSimulator( );
            var d2 = new StockDataSimulator( );

            d.AddSymbol( "Stock1", "SCK1", 240, 270 );
            d2.AddSymbol( "Stock2", "SCK2", 180, 210 );

            var obs1 = d.GetObservable( );
            var obs2 = d2.GetObservable( );

            var list1 = await obs1.Take( 5 ).ToList( );
            var list2 = await obs2.Take( 5 ).ToList( );

            Assert.Equal( expected: 5, list1.Count );
            Assert.Equal( expected: 5, list2.Count );
        }
    }
}
