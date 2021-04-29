using System.Threading;

namespace Solvedoku.ViewModels
{
    interface ISudokuViewModel
    {
        bool IsBusy { get; set; }

        Thread SudokuSolverThread { get; }
    }
}