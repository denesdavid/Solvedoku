using System;
using System.Windows.Input;

namespace Solvedoku.Commands
{
    class ParameterizedCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;
        public ParameterizedCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
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

        public bool CanExecute(object parameter)
        {
            bool isExecutable = canExecute == null ? true : canExecute(parameter);
            return isExecutable;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}