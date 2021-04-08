using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    class JigsawSudokuViewModel : ViewModelBase, ISudokuViewModel
    {
        public bool IsBusy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Thread SudokuSolverThread => throw new NotImplementedException();
    }
}