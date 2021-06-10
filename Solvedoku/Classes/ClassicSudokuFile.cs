using System;
using System.Collections.Generic;

namespace Solvedoku.Classes
{
    [Serializable]
    class ClassicSudokuFile
    {
        #region Properties

        public SudokuBoard Board { get; set; }

        public IEnumerable<SudokuBoard> Solutions { get; set; }

        #endregion

        #region Constructor

        public ClassicSudokuFile(SudokuBoard sudokuBoard, IEnumerable<SudokuBoard> solutions)
        {
            Board = sudokuBoard;
            Solutions = solutions;
        }

        #endregion    
    }
}