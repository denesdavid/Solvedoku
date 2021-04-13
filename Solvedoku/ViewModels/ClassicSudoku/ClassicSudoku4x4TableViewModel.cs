using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudoku4x4TableViewModel : ViewModelBase, IClassicSudokuTableViewModel
    {
        #region Fields
        ObservableCollection<ObservableCollection<string>>_cells = new ObservableCollection<ObservableCollection<string>>()
        {
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty},
            new ObservableCollection<string> {  string.Empty, string.Empty, string.Empty, string.Empty},
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
        #endregion
    }
}