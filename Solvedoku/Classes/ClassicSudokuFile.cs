using System;

namespace Solvedoku.Classes
{
    [Serializable]
    public class ClassicSudokuFile : BaseSudokuFile
    {
        #region Properties

        public bool AreDiagonalRulesSet { get; set; }

        #endregion
    }
}