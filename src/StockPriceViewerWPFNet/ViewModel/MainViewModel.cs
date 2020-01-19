using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace StockPriceViewerWPFNet.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _stock1Checked = false;
        private bool _stock2Checked = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsStock1SubScribed
        {
            get
            {
                return _stock1Checked;
            }
            set
            {
                _stock1Checked = value;
                RaiseChange( "IsStock1SubScribed" ); // I don't like this kind of stuff, I wouldn't do this in production.
            }
        }

        public bool IsStock2SubScribed
        {
            get
            {
                return _stock2Checked;
            }
            set
            {
                _stock2Checked = value;
                RaiseChange( "IsStock2SubScribed" );
            }
        }

        public ObservableCollection<string> ItemHistory
        {
            get;
            set;
        } = new ObservableCollection<string>( );

        public ICommand StartButton
        {
            get;
            set;
        }

        private void RaiseChange( string name )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( name ) );
        }
    }
}
