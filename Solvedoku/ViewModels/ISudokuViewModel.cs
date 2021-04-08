using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solvedoku.ViewModels
{
    interface ISudokuViewModel
    {
        bool IsBusy { get; set; }

        Thread SudokuSolverThread { get; }
    }
}