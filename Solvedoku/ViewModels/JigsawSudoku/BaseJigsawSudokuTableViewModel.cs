using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    public abstract class BaseJigsawSudokuTableViewModel : BaseSudokuTableViewModel
    {
        public abstract ObservableCollection<ObservableCollection<int>> JigsawAreas { get; set; }

        /// <summary>
        /// Converts the jigsaw puzzle areas from a collection format to a string array representation.
        /// </summary>
        /// <returns>
        /// A string array containing 9 elements, where each element represents a row of the jigsaw areas
        /// with the integers concatenated as a single string (e.g., "123456789").
        /// </returns>
        /// <remarks>
        /// Each row in the <see cref="JigsawAreas"/> collection is converted to a string by concatenating all integer values in that row.
        /// </remarks>
        public virtual string[] GetJigsawAreasAsArray()
        {
            string[] puzzleAreas = new string[9];
            for (int i = 0; i < JigsawAreas.Count; i++)
            {
                var sb = new System.Text.StringBuilder(9);
                foreach (int item in JigsawAreas[i])
                {
                    sb.Append(item);
                }
                puzzleAreas[i] = sb.ToString();
            }
            return puzzleAreas;
        }

        /// <summary>
        /// Converts the jigsaw puzzle areas from a collection format to a two-dimensional matrix representation.
        /// </summary>
        /// <returns>
        /// A 9x9 integer matrix where each element [row, column] contains the area identifier 
        /// from the corresponding position in the <see cref="JigsawAreas"/> collection.
        /// </returns>
        /// <remarks>
        /// This method iterates through the <see cref="JigsawAreas"/> collection and populates a matrix
        /// with the integer values representing the jigsaw area assignments for each cell in the puzzle.
        /// </remarks>
        public virtual int[,] GetJigsawAreasAsMatrix()
        {
            int[,] matrix = new int[9, 9];
            for (int row = 0; row < JigsawAreas.Count; row++)
            {
                for (int column = 0; column < JigsawAreas[row].Count; column++)
                {
                    matrix[row, column] = JigsawAreas[row][column];
                }
            }
            return matrix;
        }
       
    }
}