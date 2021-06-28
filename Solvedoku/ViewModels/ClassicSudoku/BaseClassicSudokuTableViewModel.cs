using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    abstract class BaseClassicSudokuTableViewModel : BaseSudokuTableViewModel
    {
        public abstract bool AreDiagonalRulesSet { get; set; }
    }
}
