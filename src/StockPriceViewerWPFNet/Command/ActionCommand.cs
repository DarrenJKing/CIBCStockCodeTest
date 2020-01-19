using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockPriceViewerWPFNet.Command
{
    public class ActionCommand : ICommand
    {
        private readonly        Action              _commandAction;

        public ActionCommand( Action commandAction ) => _commandAction = commandAction;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute( object parameter )
        {
            return true;
        }

        public void Execute( object parameter )
        {
            _commandAction?.Invoke( );
        }
    }
}
