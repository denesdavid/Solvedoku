using System;

namespace Solvedoku.Classes
{
    [Serializable]
    public class ClassicSudokuFile:SudokuFile
    {
        #region Properties

        public bool AreDiagonalRulesSet { get; set; }

        #endregion
    }
}