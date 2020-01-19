using Autofac;
using Kernel.Interface;
using StockPriceViewerWPFNet.ViewModel;
using System;
using System.Reflection;
using System.Windows;
using SymbolValue = System.Tuple<string, string, decimal>;


namespace StockPriceViewerWPFNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void Invoker( string name );
        public IStockViewerService StockViewerService { get; private set; }

        public IObservable<SymbolValue> StockObservable { get; set; }

        private MainViewModel _vm;

        public MainWindow( )
        {
            Startup( );
            InitializeComponent( );

            // Just rushed this here. To get the WPF version working. :)

            _vm = new MainViewModel( );
            DataContext = _vm;
            StockObservable.Subscribe( StockItem =>
            {
                Dispatcher.Invoke( ( ) => _vm.ItemHistory.Add( $"Name: {StockItem.Item1} Symbol: {StockItem.Item2} Value: {StockItem.Item3 } Date: {DateTime.Now}" ) );
            } );
            _vm.PropertyChanged += PropertyChanged;
        }

        private void PropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            // Demo, not doing case insensitive lookup. This would otherwise also be a framework operation that would take.
            switch( e.PropertyName )
            {
                case "IsStock1SubScribed":
                    UpdateStockSubscription( _vm.IsStock1SubScribed, "Stock1" );
                    break;
                case "IsStock2SubScribed":
                    UpdateStockSubscription( _vm.IsStock2SubScribed, "Stock2" );
                    break;
            }
        }

        private void UpdateStockSubscription( bool isSubscribed, string stockName )
        {
            var opSub = isSubscribed 
                ? ( Invoker )StockViewerService.AddStockSubscription 
                : StockViewerService.RemoveStockSubscription;
            opSub( stockName );
        }

        private void Startup( )
        {
            // Dynamically register library
            //TODO: Autofac depedency here is bad with more time this could be done on the default ServiceContainer
            var builder = new ContainerBuilder( );
            // To load different versions load different Dlls here.
            LoadLibrary( builder, "StockUtilities.dll" );
            LoadLibrary( builder, "StockPriceService.dll" );

            IContainer container = builder.Build( );

            StockViewerService = container.Resolve<IStockViewerService>( );
            StockObservable = StockViewerService.GetObservable( );
        }

        private static void LoadLibrary( ContainerBuilder builder, string name )
        {
            var dllPath = System.IO.Path.Combine( Environment.CurrentDirectory, name );
            var assembly = Assembly.LoadFile( dllPath );

            builder.RegisterAssemblyTypes( assembly ).AsImplementedInterfaces( );
        }
    }
}
