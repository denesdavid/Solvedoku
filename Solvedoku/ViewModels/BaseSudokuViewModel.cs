﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Properties;

namespace Solvedoku.ViewModels
{
    abstract class BaseSudokuViewModel: ViewModelBase
    {
        #region Fields

        protected bool _isBusy;
        protected bool _isSolutionCounterVisible;
        protected int _solutionIndex = -1;
        protected string _solutionCounter = string.Empty;
        protected string _foundSolutionCounter = string.Empty;
        protected Thread _sudokuSolverThread;
        protected UserControl _sudokuBoardControl;
        protected SudokuBoardSize _selectedSudokuBoardSize;
        protected SaveFileDialog _saveFileDialog = new SaveFileDialog();
        protected OpenFileDialog _openFileDialog = new OpenFileDialog();
        protected List<SudokuBoard> _solutions = new List<SudokuBoard>();

        #endregion

        #region Properties

        public ICommand DrawSudokuCommand { get; set; }

        public ICommand SolveSudokuCommand { get; set; }

        public ICommand SaveSudokuCommand { get; set; }

        public ICommand LoadSudokuCommand { get; set; }

        public ICommand LoadPreviousSolutionCommand { get; set; }

        public ICommand LoadNextSolutionCommand { get; set; }

        public ICommand CancelBusyCommand { get; set; }

        public ICommand BusyIndicatorLoadedCommand { get; set; }

        public Thread SudokuSolverThread 
        { 
            get => _sudokuSolverThread;
            set 
            {
                _sudokuSolverThread = value;
            }
        }

        public UserControl SudokuBoardControl
        {
            get => _sudokuBoardControl;
            set
            {
                _sudokuBoardControl = value;
                OnPropertyChanged();
            }
        }

        public SudokuBoardSize SelectedSudokuBoardSize
        {
            get => _selectedSudokuBoardSize;
            set
            {
                _selectedSudokuBoardSize = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public bool IsSolutionCounterVisible
        {
            get => _isSolutionCounterVisible;
            set
            {
                _isSolutionCounterVisible = value;
                OnPropertyChanged();
            }
        }

        public string SolutionCounter
        {
            get => _solutionCounter;
            set
            {
                _solutionCounter = value;
                OnPropertyChanged();
            }
        }

        public string FoundSolutionCounter
        {
            get => _foundSolutionCounter;
            set
            {
                _foundSolutionCounter = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        /// <summary>
        /// Determines if drawing a sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanDraw(object o) => true;

        /// <summary>
        /// Draws the sudoku table with the given size.
        /// </summary>
        protected abstract void Draw(object o);

        /// <summary>
        /// Determines if solving the sudoku is possible.
        /// </summary>
        /// <returns>True if the solutions count is 0 or at least one cell is not filled in the table.</returns>
        protected bool CanSolve() => _solutions.Count == 0
            || !GetCurrentTableViewModel().AreAllCellsFilled();

        /// <summary>
        /// Solves the sudoku.
        /// </summary>
        protected abstract void Solve();

        /// <summary>
        /// Determines if saving a sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanSave() => true;

        /// <summary>
        /// Saves the sudoku to a file.
        /// </summary>
        protected abstract void Save();

        /// <summary>
        /// Determines if loading a sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanLoad() => true;

        /// <summary>
        /// Loads a sudoku from a file.
        /// </summary>
        protected abstract void Load();

        /// <summary>
        /// Determines if loading the previous solution is possible.
        /// </summary>
        /// <returns>True if there are more solutions than 1 and
        /// the currently selected index is greater than 0.</returns>
        protected bool CanLoadPreviousSolution() => _solutions.Count > 1 && _solutionIndex > 0;

        /// <summary>
        /// Loads the next possible solution.
        /// </summary>
        protected void LoadPreviousSolution()
        {
            _solutionIndex -= 1;
            DisplayMatrixBoard(_solutions[_solutionIndex].OutputAsMatrix());
            SolutionCounter = $"{Resources.Main_SolutionsCounter} { _solutionIndex + 1 }/{ _solutions.Count }";
        }

        /// <summary>
        /// Determines if loading the next solution is possible.
        /// </summary>
        /// <returns>True if there are more solutions than 1 and
        /// the currently selected index is less than the last one.</returns>
        protected bool CanLoadNextSolution() => _solutions.Count > 1 && _solutionIndex < _solutions.Count - 1;

        /// <summary>
        /// Loads the next possible solution.
        /// </summary>
        protected void LoadNextSolution()
        {
            _solutionIndex += 1;
            DisplayMatrixBoard(_solutions[_solutionIndex].OutputAsMatrix());
            SolutionCounter = $"{Resources.Main_SolutionsCounter} { _solutionIndex + 1 }/{ _solutions.Count }";
        }

        /// <summary>
        /// Determines if cancelling the busy task is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanCancelBusy(object o) => true;

        /// <summary>
        /// Cancels the busy task.
        /// </summary>
        protected void CancelBusy(object o)
        {
            var messageBoxResult = MessageBoxService.Show(Resources.MessageBox_AbortSolution, Resources.MessageBox_Warning_Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SudokuSolverThread.Abort();
                IsBusy = false;
                bool deleteSolutions = (bool)o;
                if (deleteSolutions)
                {
                    _solutions.Clear();
                }
                else
                {
                    DisplaySolutionAndMessage();
                }           
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the command properties in the viewmodel.
        /// </summary>
        protected void LoadCommands()
        {
            DrawSudokuCommand = new ParameterizedCommand(Draw, CanDraw);
            SolveSudokuCommand = new ParameterlessCommand(Solve, CanSolve);
            SaveSudokuCommand = new ParameterlessCommand(Save, CanSave);
            LoadSudokuCommand = new ParameterlessCommand(Load, CanLoad);
            LoadPreviousSolutionCommand = new ParameterlessCommand(LoadPreviousSolution, CanLoadPreviousSolution);
            LoadNextSolutionCommand = new ParameterlessCommand(LoadNextSolution, CanLoadNextSolution);
            CancelBusyCommand = new ParameterizedCommand(CancelBusy, CanCancelBusy);
        }

        /// <summary>
        /// Returns the base viewmodel of the current sudoku board control.
        /// </summary>
        /// <returns>BaseSudokuTableViewModel</returns>
        protected virtual BaseSudokuTableViewModel GetCurrentTableViewModel() =>
            (BaseSudokuTableViewModel)SudokuBoardControl.DataContext;

        /// <summary>
        /// Creates a Sudoku board, according to the given board size.
        /// </summary>
        /// <param name="sudokuBoardSize">Size information about the board.</param>
        /// <returns>Sudoku board.</returns>
        protected virtual SudokuBoard CreateBoard(SudokuBoardSize sudokuBoardSize)
        {
            SudokuBoard sudokuBoard;
            var boardControlViewModel = (BaseSudokuTableViewModel)SudokuBoardControl.DataContext;
            if (sudokuBoardSize.BoxCountX == 3 && sudokuBoardSize.BoxCountY == 3)
            {
                sudokuBoard = SudokuFactory.ClassicWith3x3Boxes();
                for (int row = 0; row < sudokuBoardSize.Height; row++)
                {
                    string actRow = "";
                    for (int column = 0; column < sudokuBoardSize.Width; column++)
                    {
                        if (string.IsNullOrEmpty(boardControlViewModel.Cells[row][column]))
                        {
                            actRow += ".";
                        }
                        else
                        {
                            boardControlViewModel.BoldCells[row][column] = true;
                            actRow += boardControlViewModel.Cells[row][column];
                        }
                    }
                    sudokuBoard.AddRow(actRow);
                }
            }
            else
            {
                sudokuBoard = SudokuFactory.SizeAndBoxes(sudokuBoardSize);
                for (int row = 0; row < sudokuBoardSize.Height; row++)
                {
                    string actRow = "";
                    for (int column = 0; column < sudokuBoardSize.Width; column++)
                    {
                        if (string.IsNullOrEmpty(boardControlViewModel.Cells[row][column]))
                        {
                            actRow += "0";
                        }
                        else
                        {
                            boardControlViewModel.BoldCells[row][column] = true;
                            actRow += boardControlViewModel.Cells[row][column];
                        }
                    }
                    sudokuBoard.AddRow(actRow);
                }
            }
            return sudokuBoard;
        }

        /// <summary>
        /// Creates a Sudoku board, according to the given board size and special areas.
        /// </summary>
        /// <param name="sudokuBoardSize">Size information about the board.</param>
        /// <param name="areas">Special areas information about the board.</param>
        /// <returns>Sudoku board.</returns>
        protected virtual SudokuBoard CreateBoard(SudokuBoardSize sudokuBoardSize, string[] areas) 
        {
            SudokuBoard board = SudokuFactory.ClassicWithSpecialBoxes(areas);
            var boardControlViewModel = (BaseSudokuTableViewModel)SudokuBoardControl.DataContext;
            for (int row = 0; row < sudokuBoardSize.Height; row++)
            {
                string actRow = "";
                for (int column = 0; column < sudokuBoardSize.Width; column++)
                {
                    if (string.IsNullOrEmpty(boardControlViewModel.Cells[row][column]))
                    {
                        actRow += ".";
                    }
                    else
                    {
                        boardControlViewModel.BoldCells[row][column] = true;
                        actRow += boardControlViewModel.Cells[row][column];
                    }
                }
                board.AddRow(actRow);
            }
            return board;
        }

        /// <summary>
        /// Solver method for the sudoku solver thread.
        /// </summary>
        /// <param name="sudokuBoard">The Sudoku board what is need to be solved.</param>
        /// <param name="findAllSolutions">Determines if all possible solutions have to find or just one.</param>
        /// <returns>Enumerable of the solutions.</returns>
        protected IEnumerable<SudokuBoard> Sudoku_SolverThread(SudokuBoard sudokuBoard, bool findAllSolutions)
        {
            if (findAllSolutions)
            {
                foreach (var item in sudokuBoard.Solve())
                {
                   // _solutions.Add(item);
                    yield return item;
                }
               // solutions = sudokuBoard.Solve().ToList();
            }
            else
            {
                yield return sudokuBoard.Solve().First();
                yield break;
            }
        }

        /// <summary>
        /// Configures the viewmodel to display the solution(s) and a message about the possible solutions count.
        /// </summary>
        protected void DisplaySolutionAndMessage()
        {
            IsBusy = false;
            if (SudokuSolverThread.ThreadState != ThreadState.Aborted)
            {
                if (_solutions.Count > 0 && _solutions[0] != null)
                {
                    _solutionIndex = 0;
                    if (_solutions.Count > 1)
                    {
                        MessageBoxService.Show($"{Resources.MessageBox_SudokuHasMoreSolutions_Part1} { _solutions.Count}). {Resources.MessageBox_SudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                             MessageBoxButton.OK, MessageBoxImage.Information);

                        SolutionCounter = $"{Resources.Main_SolutionsCounter} { _solutionIndex + 1 }/{ _solutions.Count }";
                        IsSolutionCounterVisible = true;
                    }
                    else
                    {
                        MessageBoxService.Show(Resources.MessageBox_SudokuHasOneSolution, Resources.MessageBox_Information_Title,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    string[,] solution = _solutions[_solutionIndex].OutputAsMatrix();
                    DisplayMatrixBoard(solution);

                }
                else
                {
                    MessageBoxService.Show(Resources.MessageBox_SudokuHasNoSolution, Resources.MessageBox_Information_Title,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Loads the value from the given matrix into the selected table's viewmodel.
        /// </summary>
        /// <param name="board"></param>
        protected void DisplayMatrixBoard(string[,] board)
        {
            var boardControlViewModel = (BaseSudokuTableViewModel)SudokuBoardControl.DataContext;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    if (board[row, column] != "0")
                    {
                        boardControlViewModel.Cells[row][column] = board[row, column];
                    }
                }
            }
        }
        #endregion
    }
}