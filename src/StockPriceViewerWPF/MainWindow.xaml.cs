using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using Kernel.Interface;
using StockPriceViewerWPF.ViewModel;
using StockPriceViewerWPF.Command;
using SymbolValue = System.Tuple<string, string, decimal>;

namespace StockPriceViewerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            _vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged( object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            if( e.PropertyName.Equals( "IsStock1SubScribed" ) )
            {
                if( _vm.IsStock1SubScribed )
                {
                    StockViewerService.AddStockSubscription( "Stock1" );
                }
                else
                {
                    StockViewerService.RemoveStockSubscription( "Stock1" );
                }
            }

            if( e.PropertyName.Equals( "IsStock2SubScribed" ) )
            {
                if( _vm.IsStock2SubScribed )
                {
                    StockViewerService.AddStockSubscription( "Stock2" );
                }
                else
                {
                    StockViewerService.RemoveStockSubscription( "Stock2" );
                }
            }
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
