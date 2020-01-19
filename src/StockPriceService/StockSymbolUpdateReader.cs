using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;
using Kernel.Interface;
using StockUtilities;

[assembly: InternalsVisibleTo( "StockPriceService.Test" )]
namespace StockPriceService
{
    public class StockSymbolUpdateReader : IStockViewerService
    {
        private readonly    IStockDataProvider          _stockProvider;

        public StockSymbolUpdateReader( IStockDataProvider provider ) => _stockProvider = provider;

        public void AddStockSubscription( string name )
        {
            _stockProvider.AddStockSubscription( name );
        }

        public void RemoveStockSubscription( string name )
        {
            _stockProvider.RemoveStockSubscription( name );
        }

        public IObservable<Tuple<string, string, decimal>> GetObservable( )
        {
            return _stockProvider.GetObservable( );
        }
    }
}
