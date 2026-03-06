using System.Collections.ObjectModel;
using System.Linq;

namespace Solvedoku.ViewModels.SudokuTables.Jigsaw
{
    class JigsawSudoku9x9TableViewModel : BaseJigsawSudokuTableViewModel
    {
        #region Fields

        ObservableCollection<ObservableCollection<string>> _cells = new(
           Enumerable.Range(0, 9).Select(_ =>
               new ObservableCollection<string>(Enumerable.Repeat(string.Empty, 9))
           )
       );

        ObservableCollection<ObservableCollection<bool>> _boldCells = new(
            Enumerable.Range(0, 9).Select(_ =>
                new ObservableCollection<bool>(Enumerable.Repeat(false, 9))
            )
        );

        ObservableCollection<ObservableCollection<int>> _jigsawAreas = new(
            Enumerable.Range(0, 9).Select(_ =>
                new ObservableCollection<int>(Enumerable.Repeat(-1, 9))
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

        public override ObservableCollection<ObservableCollection<int>> JigsawAreas
        { 
            get => _jigsawAreas;
            set
            {
                _jigsawAreas = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}