using System.Collections.ObjectModel;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    public abstract class BaseJigsawSudokuTableViewModel : BaseSudokuTableViewModel
    {
        public abstract ObservableCollection<ObservableCollection<int>> JigsawAreas { get; set; }

        public virtual string[] GetJigsawAreasAsArray()
        {
            string[] puzzleAreas = new string[9];
            int i = -1;
            foreach (ObservableCollection<int> row in JigsawAreas)
            {
                i++;
                string actRow = "";
                foreach (int item in row)
                {
                    actRow += item;
                }
                puzzleAreas[i] = actRow;
            }
            return puzzleAreas;
        }

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