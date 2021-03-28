namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudoku6x6TableViewModel : ViewModelBase, IClassicSudokuTableViewModel
    {
        #region Fields
        string[][] _cells = new string[6][]
        {
            new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[6] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
        };
        #endregion

        #region Properties
        public string[][] Cells
        {
            get => _cells;
            set
            {
                _cells = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}