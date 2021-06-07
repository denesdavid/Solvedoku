using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    interface IJigsawSudokuTableViewModel
    {
        ObservableCollection<ObservableCollection<string>> Cells { get; set; }

        ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        ObservableCollection<ObservableCollection<int>> JigsawAreas { get; set; }

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

        string[] GetJigsawAreasAsArray();

        int[,] GetJigsawAreasAsMatrix();
       
    }
}