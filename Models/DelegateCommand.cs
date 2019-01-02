using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Observer.Models
{
    /// <summary>
    /// Класс описывающий команды
    /// </summary>
    class DelegateCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<object> _execute, Func<object, bool> _canExecute = null)
        {
            execute = _execute;
            canExecute = _canExecute;
        }
        
        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
    }
}
