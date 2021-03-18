using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace Solvedoku.Classes
{
    [Serializable]
    class PuzzleSudokuFile
    {
        #region Fields

        private SudokuBoardSize _sudokuBoardSize;
        private SudokuBoard _sudokuBoard;
        private string[] _boardAreas;
        private List<SudokuBoard> _solutions = new List<SudokuBoard>();
        private ObservableCollection<ColorItem> _colorList;

        #endregion

        #region Properties
        public SudokuBoard Board
        {
            get { return _sudokuBoard; }
            set { _sudokuBoard = value; }
        }
        public string [] Areas
        {
            get { return _boardAreas; }
            set { _boardAreas = value; }
        }
        public List<SudokuBoard> Solutions
        {
            get { return _solutions; }
            set { _solutions = value; }
        }
        public SudokuBoardSize BoardSize
        {
            get { return _sudokuBoardSize; }
            set { _sudokuBoardSize = value; }
        }
        public ObservableCollection<ColorItem> ColorList
        {
            get { return _colorList; }
            set { _colorList = value; }
        }
        #endregion
    }
}