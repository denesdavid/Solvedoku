using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace Solvedoku.Classes
{
    [Serializable]
    class SudokuPuzzleHandler
    {
        #region Fields

        private int _selectedSizeIndex;
        private SudokuBoard _actBoard;
        private string[] _actAreas;
        private List<SudokuBoard> _puzzleSolutions = new List<SudokuBoard>();
        private ObservableCollection<ColorItem> _colorList;

        #endregion

        #region Properties
        public SudokuBoard ActPuzzleBoard
        {
            get { return _actBoard; }
            set { _actBoard = value; }
        }
        public string [] ActAreas
        {
            get { return _actAreas; }
            set { _actAreas = value; }
        }
        public List<SudokuBoard> ActPuzzleSolutions
        {
            get { return _puzzleSolutions; }
            set { _puzzleSolutions = value; }
        }
        public int SelectedSizeIndex
        {
            get { return _selectedSizeIndex; }
            set { _selectedSizeIndex = value; }
        }
        public ObservableCollection<ColorItem> ColorList
        {
            get { return _colorList; }
            set { _colorList = value; }
        }
        #endregion
    }
}