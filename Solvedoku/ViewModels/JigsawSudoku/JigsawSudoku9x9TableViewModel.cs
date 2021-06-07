using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    class JigsawSudoku9x9TableViewModel : ViewModelBase, IJigsawSudokuTableViewModel
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

        ObservableCollection<ObservableCollection<int>> _puzzleAreas = new ObservableCollection<ObservableCollection<int>>()
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

        public ObservableCollection<ObservableCollection<string>> Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObservableCollection<bool>> BoldCells
        {
            get => _boldCells;
            set
            {
                _boldCells = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObservableCollection<int>> JigsawAreas
        { 
            get => _puzzleAreas;
            set
            {
                _puzzleAreas = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Functions
        public bool AreAllCellsFilled()
        {
            foreach (var row in Cells)
            {
                foreach (var column in row)
                {
                    if (column == string.Empty)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool AreAnyCellsFilled()
        {
            foreach (var row in Cells)
            {
                foreach (var column in row)
                {
                    if (column != string.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public string[] GetJigsawAreasAsArray()
        {
            string[] puzzleAreas = new string[9];
            int i = -1;
            foreach (ObservableCollection<int> row in JigsawAreas)
            {
                i++;
                string actRow = "";
                foreach (int item in row)
                {
                    actRow += item;
                }
                puzzleAreas[i] = actRow;
            }
            return puzzleAreas;
        }

        public int[,] GetJigsawAreasAsMatrix()
        {
            int[,] matrix = new int[9, 9];
            for (int row = 0; row < JigsawAreas.Count; row++)
            {
                for (int column = 0; column < JigsawAreas[row].Count; column++)
                {
                    matrix[row, column] = JigsawAreas[row][column];
                }
            }
            return matrix;
        }

        #endregion
    }
}