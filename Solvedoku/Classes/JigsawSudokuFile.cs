using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace Solvedoku.Classes
{
    [Serializable]
    class JigsawSudokuFile
    {
        #region Fields

        SudokuBoardSize _sudokuBoardSize;
        SudokuBoard _sudokuBoard;
        int[,] _boardAreas;
        List<SudokuBoard> _solutions = new List<SudokuBoard>();
        ObservableCollection<ColorItem> _colorList;

        #endregion

        #region Properties
        public SudokuBoard Board
        {
            get { return _sudokuBoard; }
            set { _sudokuBoard = value; }
        }
        public int [,] Areas
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

        public JigsawSudokuFile(SudokuBoard board, SudokuBoardSize boardSize, int[,] areas, List<SudokuBoard> solutions)
        {
            Board = board;
            BoardSize = boardSize;
            Areas = areas;
            Solutions = solutions;
        }
    }
}