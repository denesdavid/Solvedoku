namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudoku9x9TableViewModel : ViewModelBase, IClassicSudokuTableViewModel
    {
        #region Fields
        string[][] _cells = new string[9][]
        {
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
            new string[9] {  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty},
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