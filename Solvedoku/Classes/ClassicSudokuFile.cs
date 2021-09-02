using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Solvedoku.Classes
{
    [Serializable]
    public class ClassicSudokuFile
    {
        #region Properties

        public bool AreDiagonalRulesSet { get; set; }

        public int SolutionIndex { get; set; }

        public string SolutionCounter { get; set; }

        public bool IsSolutionCounterVisible { get; set; }

        public SudokuBoard Board { get; set; }

        public ObservableCollection<ObservableCollection<string>> Cells { get; set; }

        public ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        public SudokuBoardSize SelectedSudokuBoardSize { get; set; }
       
        public List<SudokuBoard> Solutions { get; set; }

        #endregion
    }
}