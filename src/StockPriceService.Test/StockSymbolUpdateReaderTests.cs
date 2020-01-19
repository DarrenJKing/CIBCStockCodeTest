using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StockPriceService.Test
{
    public class StockSymbolUpdateReaderTests
    {
        [Fact]
        public void Create_Test( )
        {
            new StockSymbolUpdateReader( null );
        }
    }
}
