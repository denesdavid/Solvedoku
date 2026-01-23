using System;
using System.Collections.ObjectModel;

namespace Solvedoku.Classes
{
    [Serializable]
    class JigsawSudokuFile : BaseSudokuFile
    {
        #region Properties

        public int[,] Areas { get; set; }

        public ObservableCollection<ObservableCollection<bool>> BoldCells { get; set; }

        #endregion
    }
}