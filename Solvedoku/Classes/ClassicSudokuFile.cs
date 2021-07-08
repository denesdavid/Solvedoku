using System;
using System.Collections.Generic;

namespace Solvedoku.Classes
{
    [Serializable]
    class ClassicSudokuFile
    {
        #region Properties

        public SudokuBoard Board { get; set; }

        public List<SudokuBoard> Solutions { get; set; }

        #endregion

        #region Constructor

        public ClassicSudokuFile(SudokuBoard sudokuBoard, List<SudokuBoard> solutions)
        {
            Board = sudokuBoard;
            Solutions = solutions;
        }

        #endregion    
    }
}