using System;
using System.Collections.Generic;
using System.Text;
using SymbolValue = System.Tuple<string, string, decimal>;

namespace Kernel.Interface
{
    public interface IStockViewerService
    {
        IObservable<SymbolValue> GetObservable( );

        void AddStockSubscription( string name );

        void RemoveStockSubscription( string name );
    }
}
