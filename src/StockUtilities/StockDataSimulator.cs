using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using Kernel.Interface;
using SymbolValue = System.Tuple<string, string, decimal>;

[assembly: InternalsVisibleTo( "StockUtilities.Test" )]
namespace StockUtilities
{
    public class StockDataSimulator : IStockDataProvider, IDisposable
    {
        // Timer was used to create events because it will use the threadpool
        private readonly    List<(string name, Timer timer)>    _timers                 = new List<(string name, Timer timer)>( );
        private             IObserver<SymbolValue>              _observer;
        private readonly    IObservable<SymbolValue>            _observable;

        public StockDataSimulator( )
        {
            _observable = Observable.Create<SymbolValue>( SetObserver );
        }

        public IObservable<SymbolValue> GetObservable( )
        {
            return _observable;
        }

        public void AddStockSubscription( string stockName )
        {
            switch( stockName.ToLower( ) )
            {
                case "stock1":
                    AddSymbol( new SimulationItemSetting( )
                    {
                        Name = "Stock1",
                        Symbol = "STK1",
                        RateOfChange = 0.3m,
                        LastValue = 255,
                        PriceHigh = 270,
                        PriceLow = 240,
                        ReportIntervalStart = 500,
                        ReportIntevalEnd = 5000
                    } );
                    break;
                case "stock2":
                    AddSymbol( new SimulationItemSetting( )
                    {
                        Name = "Stock2",
                        Symbol = "STK2",
                        RateOfChange = 0.3m,
                        LastValue = 190m,
                        PriceHigh = 210m,
                        PriceLow = 180m,
                        ReportIntervalStart = 500,
                        ReportIntevalEnd = 5000
                    } );
                    break;
                default:
                    AddSymbol( new SimulationItemSetting( )
                    {
                        Name = "Blackrock",
                        Symbol = "XIT",
                        RateOfChange = 0.3m,
                        LastValue = 25.0m,
                        PriceHigh = 26.9m,
                        PriceLow = 24.2m,
                        ReportIntervalStart = 500,
                        ReportIntevalEnd = 5000
                    } );
                    break;
            }
        }

        public void RemoveStockSubscription( string name )
        {
            var tup = _timers.FirstOrDefault( t => t.name.Equals( name, StringComparison.InvariantCultureIgnoreCase ) );
            _timers.Remove( tup );
            CleanUpTimer( tup );
        }

        public void Dispose( )
        {
            _timers.ForEach( CleanUpTimer );
        }

        private StockDataSimulator SetObserver( IObserver<SymbolValue> observer )
        {
            _observer = observer;
            return this;
        }

        private void CleanUpTimer( (string Name, Timer timer) t )
        {
            t.timer.Stop( );
            t.timer.Dispose( );
        }

        internal void AddSymbol( SimulationItemSetting setting )
        {
            // For this demo no need to keep track of setting at this point not needed to for updating simulation behavior
            // timers dispose easily
            var random = new Random( );
            var t = new Timer( )
            {
                Interval = 100
            };
            t.Elapsed += ( sobject, eventArgs ) =>
            {
                // lots of capture here. Would be a place to refactor
                t.Interval = random.Next( setting.ReportIntervalStart, setting.ReportIntevalEnd );
                decimal lastVal = setting.LastValue;
                int randomFlag = random.Next( 2 );
                setting.LastValue += randomFlag == 1
                    ? setting.RateOfChange
                    : -setting.RateOfChange;
                
                // Limit
                if( setting.LastValue > setting.PriceHigh )
                {
                    setting.LastValue = setting.PriceHigh;
                }
                else if( setting.LastValue < setting.PriceLow )
                {
                    setting.LastValue = setting.PriceLow;
                }
                if( lastVal != setting.LastValue )
                {
                    _observer?.OnNext( Tuple.Create( setting.Name, setting.Symbol, setting.LastValue ) );
                }
            };
            t.Start( );
            _timers.Add( ( setting.Name, t ) );
        }
    }
}
