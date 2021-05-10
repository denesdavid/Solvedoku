using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudoku9x9TableViewModel : ViewModelBase, IClassicSudokuTableViewModel
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
        #endregion
    }
}