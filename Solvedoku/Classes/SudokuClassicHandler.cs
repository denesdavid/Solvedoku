using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Solvedoku.Classes
{
    [Serializable]
    class SudokuClassicHandler
    {
        #region Fields

        private int _selectedSizeIndex;
        private SudokuBoard _actBoard;
        private List<SudokuBoard> _classicSolutions = new List<SudokuBoard>();

        #endregion

        #region Constructor

        public SudokuClassicHandler()
        {
            
        }

        #endregion

        #region Methods

        public void Save(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, this);
            }
        }

        #endregion

        #region Properties

        public SudokuBoard ActClassicBoard
        {
            get { return _actBoard; }
            set { _actBoard = value; }
        }

        public List<SudokuBoard> ActClassicSolutions
        {
            get { return _classicSolutions; }
            set { _classicSolutions = value; }
        }

        public int SelectedSizeIndex
        {
            get { return _selectedSizeIndex; }
            set { _selectedSizeIndex = value; }
        }

        #endregion
    }
}
