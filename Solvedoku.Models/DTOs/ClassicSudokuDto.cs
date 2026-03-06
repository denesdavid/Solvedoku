using System;

namespace Solvedoku.Classes
{
    [Serializable]
    public class ClassicSudokuDto : BaseSudokuFile
    {
        #region Properties

        public bool AreDiagonalRulesSet { get; set; }

        #endregion
    }
}