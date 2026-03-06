using System.Collections.ObjectModel;
using System.Linq;

namespace Solvedoku.ViewModels.SudokuTables.Classic
{
    class ClassicSudoku6x6TableViewModel : BaseClassicSudokuTableViewModel
    {
        #region Fields

        bool _areDiagonalRulesSet = false;

        ObservableCollection<ObservableCollection<string>> _cells = new(
            Enumerable.Range(0, 6).Select(_ =>
                new ObservableCollection<string>(Enumerable.Repeat(string.Empty, 6))
            )
        );

        ObservableCollection<ObservableCollection<bool>> _boldCells = new(
           Enumerable.Range(0, 6).Select(_ =>
               new ObservableCollection<bool>(Enumerable.Repeat(false, 6))
           )
        );

        #endregion

        #region Properties

        public override ObservableCollection<ObservableCollection<string>> Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                OnPropertyChanged();
            }
        }

        public override ObservableCollection<ObservableCollection<bool>> BoldCells
        {
            get => _boldCells;
            set
            {
                _boldCells = value;
                OnPropertyChanged();
            }
        }

        public override bool AreDiagonalRulesApplied
        {
            get => _areDiagonalRulesSet;
            set
            {
                _areDiagonalRulesSet = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}