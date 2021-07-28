using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Solvedoku.Classes
{
    [Serializable]
    class ClassicSudokuFile
    {
        #region Properties

        public SudokuBoard Board { get; set; }

        public ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }
       
        public List<SudokuBoard> Solutions { get; set; }

        #endregion

        #region Constructor

        public ClassicSudokuFile(){}

        public ClassicSudokuFile(SudokuBoard sudokuBoard, ObservableCollection<ObservableCollection<bool>> boldCells, List<SudokuBoard> solutions)
        {
            Board = sudokuBoard;
            BoldCells = boldCells;
            Solutions = solutions;
        }

        #endregion    
    }
}