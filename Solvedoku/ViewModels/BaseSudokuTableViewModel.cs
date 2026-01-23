using System.Collections.ObjectModel;
using System.Linq;

namespace Solvedoku.ViewModels
{
    public abstract class BaseSudokuTableViewModel : ViewModelBase
    {

        #region Properties

        public abstract ObservableCollection<ObservableCollection<string>> Cells { get; set; }

        public abstract ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        #endregion

        #region Functions

        /// <summary>
        /// Determines if all cells are filled in the board.
        /// </summary>
        /// <returns>True if all cells are filled.</returns>
        public bool AreAllCellsFilled()
        {
            return Cells.All(row => row.All(cell => !string.IsNullOrEmpty(cell)));
        }

        /// <summary>
        /// Determines if at least one cell is filled in the board.
        /// </summary>
        /// <returns>True if at least one cell is filled.</returns>
        public bool AreAnyCellsFilled()
        {
            return Cells.Any(row => row.Any(cell => !string.IsNullOrEmpty(cell)));
        }

        #endregion
    }
}