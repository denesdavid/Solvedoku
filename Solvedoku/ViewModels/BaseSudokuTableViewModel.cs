using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels
{
    abstract class BaseSudokuTableViewModel : ViewModelBase
    {
        public abstract ObservableCollection<ObservableCollection<string>> Cells { get; set; }

        public abstract ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        /// <summary>
        /// Determines if all cells are filled in the board.
        /// </summary>
        /// <returns>True if all cells are filled.</returns>
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

        /// <summary>
        /// Determines if at least one cell is filled in the board.
        /// </summary>
        /// <returns>True if at least one cell is filled.</returns>
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
    }
}