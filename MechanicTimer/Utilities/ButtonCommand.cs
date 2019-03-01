using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MechanicTimer.Utilities
{
    public class ButtonCommand : ICommand
    {
        private readonly Action<object> execute;
        public void Execute(object param)
        {
            execute(param);
        }

        private readonly Predicate<object> canExecute;
        public bool CanExecute(object param)
        {
            if (canExecute != null)
            {
                return canExecute(param);
            }
            return true;
        }

        public ButtonCommand(Action<object> action, Predicate<object> predicate)
        {
            execute = action;
            canExecute = predicate;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
