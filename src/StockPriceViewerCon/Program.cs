using System;
using System.IO;
using System.Reflection;
using Autofac;
using Kernel.Interface;
using static System.Console;

namespace StockPriceViewerCon
{
    class Program
    {
        private static      IStockViewerService         _stockService;

        static void Main( string[ ] args )
        {
            Startup( );
            DisplayStock( );
            WriteLine( "Press Enter to Exit..." );
            ReadLine( );
        }

        private static void DisplayStock( )
        {
            _stockService.GetObservable( ).Subscribe( tuple =>
            {
                WriteLine( $"Quote Update: Name:{tuple.Item1} Symbol:{tuple.Item2} Value:{tuple.Item3} {DateTime.Now}" );
            } );
            _stockService.AddStockSubscription( "Stock1" );
            _stockService.AddStockSubscription( "Stock2" );
        }

        private static void Startup( )
        {
            // Dynamically register library
            //TODO: Autofac depedency here is bad with more time this could be done on the default ServiceContainer
            var builder = new ContainerBuilder( );
            // To load different versions load different Dlls here.
            LoadLibrary( builder, "StockUtilities.dll" );
            LoadLibrary( builder, "StockPriceService.dll" );
            
            IContainer container = builder.Build( );
            _stockService = container.Resolve<IStockViewerService>( );
        }

        private static void LoadLibrary( ContainerBuilder builder, string name )
        {
            var dllPath = Path.Combine( Environment.CurrentDirectory, name );
            var assembly = Assembly.LoadFile( dllPath );
            
            builder.RegisterAssemblyTypes( assembly ).AsImplementedInterfaces( );
        }
    }
}
