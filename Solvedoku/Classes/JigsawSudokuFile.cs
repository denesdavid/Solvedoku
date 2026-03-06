using System;

namespace Solvedoku.Classes
{
    [Serializable]
    class JigsawSudokuFile:SudokuFile
    {
        #region Properties

        public int[,] Areas { get; set; }

        #endregion
    }
}