using System;
using System.Windows.Input;

namespace Solvedoku.Commands
{
    class ParameterlessCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

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

        public bool CanExecute(object parameter = null)
        {
            bool isExecutable = _canExecute == null ? true : _canExecute();
            return isExecutable;
        }

        public void Execute(object parameter = null)
        {
            _execute();
        }
    }
}