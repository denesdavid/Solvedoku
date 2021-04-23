using System;
using System.Windows.Input;

namespace Solvedoku.Commands
{
    class ParameterizedCommand : ICommand
    {
        Action<object> _execute;
        Predicate<object> _canExecute;
        public ParameterizedCommand(Action<object> execute, Predicate<object> canExecute)
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

        public bool CanExecute(object parameter) => _canExecute == null ? true : _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
}