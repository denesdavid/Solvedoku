using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace Solvedoku.Classes
{
    [Serializable]
    class JigsawSudokuFile
    {
        #region Properties

        public SudokuBoard Board { get; set; }

        public int[,] Areas { get; set; }
     
        public List<SudokuBoard> Solutions { get; set; }
        
        public SudokuBoardSize BoardSize { get; set; }

        public ObservableCollection<ColorItem> ColorList { get; set; }

        #endregion

        #region Constructor
        public JigsawSudokuFile(SudokuBoard board, SudokuBoardSize boardSize, int[,] areas, List<SudokuBoard> solutions)
        {
            Board = board;
            BoardSize = boardSize;
            Areas = areas;
            Solutions = solutions;
        }
        #endregion
    }
}