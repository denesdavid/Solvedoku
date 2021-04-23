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

        SudokuBoardSize _sudokuBoardSize;
        SudokuBoard _sudokuBoard;
        string[] _boardAreas;
        List<SudokuBoard> _solutions = new List<SudokuBoard>();
        ObservableCollection<ColorItem> _colorList;

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