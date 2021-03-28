using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    interface IClassicSudokuTableViewModel
    {
        string[][] Cells { get; set; }
    }
}