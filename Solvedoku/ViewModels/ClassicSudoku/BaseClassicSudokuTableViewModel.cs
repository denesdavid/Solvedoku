using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    public abstract class BaseClassicSudokuTableViewModel : BaseSudokuTableViewModel
    {
        public override abstract ObservableCollection<ObservableCollection<string>> Cells { get; set; }
        public override abstract ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }
        public abstract bool AreDiagonalRulesApplied { get; set; }
    }
}