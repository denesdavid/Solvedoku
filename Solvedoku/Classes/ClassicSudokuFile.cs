using System;
using System.Collections.Generic;

namespace Solvedoku.Classes
{
    [Serializable]
    class ClassicSudokuFile
    {
        #region Fields

        private SudokuBoardSize _sudokuBoardSize;
        private SudokuBoard _actBoard;
        private List<SudokuBoard> _classicSolutions = new List<SudokuBoard>();

        #endregion

        #region Constructor

        public ClassicSudokuFile()
        {
            
        }

        #endregion

        #region Properties

        public SudokuBoard Board
        {
            get { return _actBoard; }
            set { _actBoard = value; }
        }

        public List<SudokuBoard> Solutions
        {
            get { return _classicSolutions; }
            set { _classicSolutions = value; }
        }

        public SudokuBoardSize BoardSize
        {
            get { return _sudokuBoardSize; }
            set { _sudokuBoardSize = value; }
        }

        #endregion
    }
}