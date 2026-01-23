using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    public abstract class BaseClassicSudokuTableViewModel : BaseSudokuTableViewModel
    {
        #region Properties

        public override abstract ObservableCollection<ObservableCollection<string>> Cells { get; set; }
        public override abstract ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        #endregion

        #region Constructor

        public abstract bool AreDiagonalRulesApplied { get; set; }

        #endregion
    }
}