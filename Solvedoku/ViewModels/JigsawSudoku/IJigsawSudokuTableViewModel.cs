using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    interface IJigsawSudokuTableViewModel
    {
        ObservableCollection<ObservableCollection<string>> Cells { get; set; }
    }
}