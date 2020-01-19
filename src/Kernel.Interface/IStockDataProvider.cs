using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Interface
{
    public interface IStockDataProvider
    {
        IObservable<Tuple<string, string, decimal>> GetObservable( );

        void AddStockSubscription( string name );

        void RemoveStockSubscription( string name );
    }
}
