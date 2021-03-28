namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudoku4x4TableViewModel : ViewModelBase, IClassicSudokuTableViewModel
    {
        #region Fields
        string[][] _cells = new string[4][]
        {
            new string[4] {  string.Empty, string.Empty, string.Empty, string.Empty},
            new string[4] {  string.Empty, string.Empty, string.Empty, string.Empty},
            new string[4] {  string.Empty, string.Empty, string.Empty, string.Empty},
            new string[4] {  string.Empty, string.Empty, string.Empty, string.Empty},
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