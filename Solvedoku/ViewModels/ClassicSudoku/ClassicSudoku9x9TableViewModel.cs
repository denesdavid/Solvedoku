using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudoku9x9TableViewModel : BaseClassicSudokuTableViewModel
    {
        #region Fields

        bool _areDiagonalRulesSet = false;

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

        public override bool AreDiagonalRulesSet
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