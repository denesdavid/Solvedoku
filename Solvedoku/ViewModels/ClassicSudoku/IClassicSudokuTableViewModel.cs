using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    interface IClassicSudokuTableViewModel
    {
        ObservableCollection<ObservableCollection<string>> Cells { get; set; }

        ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        /// <summary>
        /// Determines if all cells are filled in the board.
        /// </summary>
        /// <returns>True if all cells are filled.</returns>
        bool AreAllCellsFilled();

        /// <summary>
        /// Determines if at least one cell is filled in the board.
        /// </summary>
        /// <returns>True if at least one cell is filled.</returns>
        bool AreAnyCellsFilled();
    }
}