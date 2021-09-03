using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    class JigsawSudoku9x9TableViewModel : BaseJigsawSudokuTableViewModel
    {
        #region Fields

        ObservableCollection<ObservableCollection<string>> _cells = new ObservableCollection<ObservableCollection<string>>()
        {
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
        };

        ObservableCollection<ObservableCollection<bool>> _boldCells = new ObservableCollection<ObservableCollection<bool>>()
        {
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool> {  false, false, false, false, false, false, false, false, false},
        };

        ObservableCollection<ObservableCollection<int>> _jigsawAreas = new ObservableCollection<ObservableCollection<int>>()
        {
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
            new ObservableCollection<int> {  -1, -1, -1, -1, -1, -1, -1, -1, -1},
        };

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