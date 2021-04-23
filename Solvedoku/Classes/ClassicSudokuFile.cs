using System;
using System.Collections.Generic;

namespace Solvedoku.Classes
{
    [Serializable]
    class ClassicSudokuFile
    {
        #region Fields

        SudokuBoard _sudokuBoard;
        IEnumerable<SudokuBoard> _solutions = new List<SudokuBoard>();

        #endregion

        #region Constructor

        public ClassicSudokuFile(SudokuBoard sudokuBoard, IEnumerable<SudokuBoard> solutions)
        {
            _sudokuBoard = sudokuBoard;
            _solutions = solutions;
        }

        #endregion

        #region Properties

        public SudokuBoard Board
        {
            get { return _sudokuBoard; }
            set { _sudokuBoard = value; }
        }

        public IEnumerable<SudokuBoard> Solutions
        {
            get { return _solutions; }
            set { _solutions = value; }
        }
        #endregion
    }
}