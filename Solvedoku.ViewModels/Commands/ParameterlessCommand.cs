using System;
using System.Windows.Input;

namespace Solvedoku.Commands
{
    public class ParameterlessCommand : ICommand
    {
        Action _execute;
        Func<bool> _canExecute;

        public ParameterlessCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
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

        public bool CanExecute(object parameter = null) => _canExecute == null ? true : _canExecute();

        public void Execute(object parameter = null) => _execute();
    }
}