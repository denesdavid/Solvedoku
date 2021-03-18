using Microsoft.Win32;
using Solvedoku.Views.ClassicSudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudokuViewModel : ViewModelBase
    {
        #region Fields
        private UserControl _sudokuTable;
        private string _solvesCount = string.Empty;
        private SaveFileDialog _saveFileDialog = new SaveFileDialog();
        private OpenFileDialog _openFileDialog = new OpenFileDialog();
        #endregion

        #region Properties

        public ICommand DrawClassicSudokuCommand { get; set; }

        public ICommand SolveClassicSudokuCommand { get; set; }

        public ICommand SaveClassicSudokuCommand { get; set; }

        public ICommand LoadClassicSudokuCommand { get; set; }

        public UserControl SudokuTable 
        {
            get => _sudokuTable;
            set
            {
                _sudokuTable = value;
                OnPropertyChanged();
            }
        }

        public string SolvesCount 
        {
            get => _solvesCount;
            set
            {
                _solvesCount = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor

        public ClassicSudokuViewModel()
        {
            LoadCommands();
            SudokuTable = new UcClassicSudoku9x9Table();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Determines if drawing a classic sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanDraw() => true;

        /// <summary>
        /// Draws the classic sudoku table with the given size.
        /// </summary>
        void Draw()
        {
            
        }

        /// <summary>
        /// Determines if solving the sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanSolve() => true;

        /// <summary>
        /// Solves the classic sudoku.
        /// </summary>
        void Solve()
        {
            
        }

        /// <summary>
        /// Determines if saving a classic sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanSave() => true;

        /// <summary>
        /// Saves a classic sudoku to a file.
        /// </summary>
        void Save()
        {

        }

        /// <summary>
        /// Determines if loading a classic sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanLoad() => true;

        /// <summary>
        /// Loads a classic sudoku from a file.
        /// </summary>
        void Load()
        {
            
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the command properties in the viewmodel.
        /// </summary>
        private void LoadCommands()
        {
            /*GetFileListCommand = new ParameterlessCommand(GetFileListBySelectedItem, CanGetFileListBySelectedItem);
            OpenCommand = new ParameterlessCommand(Open, CanOpen);
            CopyCommand = new ParameterlessCommand(Copy, CanCopy);
            RenameCommand = new ParameterlessCommand(Rename, CanRename);
            DeleteCommand = new ParameterlessCommand(Delete, CanDelete);
            CreateNewFileCommand = new ParameterlessCommand(CreateNewFile, CanCreateNewFile);
            CreateNewDirectoryCommand = new ParameterlessCommand(CreateNewDirectory, CanCreateNewDirectory);
            ChangeAttributesCommand = new ParameterlessCommand(ChangeAttributes, CanChangeAttributes);
            WriteInFileCommand = new ParameterlessCommand(WriteInFile, CanWriteInFile);
            UploadFileCommand = new ParameterlessCommand(UploadFile, CanUploadFile);
            DownloadFileCommand = new ParameterlessCommand(DownloadFile, CanDownloadFile);*/
        }
        #endregion
    }
}