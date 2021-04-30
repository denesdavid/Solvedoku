using System.Threading;
using System.Windows.Input;

namespace Solvedoku.ViewModels
{
    interface ISudokuViewModel
    {
        bool IsBusy { get; set; }

        ICommand CancelBusyCommand { get; set; }

        Thread SudokuSolverThread { get; }
    }
}