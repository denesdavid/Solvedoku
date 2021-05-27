﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Properties;
using Solvedoku.Views.ClassicSudoku;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudokuViewModel : ViewModelBase
    {
        #region Fields
        bool _isBusy;
        bool _isSolutionsCountVisible;
        int _solutionIndex = -1;
        string _solutionsCount = string.Empty;
        Thread _sudokuSolverThread;
        UserControl _sudokuBoardControl;
        SudokuBoardSize _selectedSudokuBoardSize;
        SaveFileDialog _saveFileDialog = new SaveFileDialog();
        OpenFileDialog _openFileDialog = new OpenFileDialog();
        List<SudokuBoard> _solutions = new List<SudokuBoard>();
        #endregion

        #region Properties

        public ICommand DrawClassicSudokuCommand { get; set; }

        public ICommand SolveClassicSudokuCommand { get; set; }

        public ICommand SaveClassicSudokuCommand { get; set; }

        public ICommand LoadClassicSudokuCommand { get; set; }

        public ICommand LoadPreviousSolutionCommand { get; set; }

        public ICommand LoadNextSolutionCommand { get; set; }

        public ICommand CancelBusyCommand { get; set; }

        public ICommand BusyIndicatorLoadedCommand { get; set; }

        public Thread SudokuSolverThread { get => _sudokuSolverThread; }

        public UserControl SudokuBoardControl
        {
            get => _sudokuBoardControl;
            set
            {
                _sudokuBoardControl = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SudokuBoardSize> SudokuBoardSizes
        {
            get => new ObservableCollection<SudokuBoardSize> {
                new SudokuBoardSize{
                    Width = 9,
                    Height = 9,
                    BoxCountX = 3,
                    BoxCountY = 3
                },
                new SudokuBoardSize{
                    Width = 6,
                    Height = 6,
                    BoxCountX = 3,
                    BoxCountY = 2
                },
                new SudokuBoardSize{
                    Width = 4,
                    Height = 4,
                    BoxCountX = 2,
                    BoxCountY = 2
                }
            };
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

        public bool IsSolutionsCountVisible 
        {
            get => _isSolutionsCountVisible;
            set
            {
                _isSolutionsCountVisible = value;
                OnPropertyChanged();
            }
        }

        public string SolutionsCount
        {
            get => _solutionsCount;
            set
            {
                _solutionsCount = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor

        public ClassicSudokuViewModel()
        {
            LoadCommands();
            SudokuBoardControl = new UcClassicSudoku9x9Table();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Determines if drawing a classic sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanDraw(object o) => true;

        /// <summary>
        /// Draws the classic sudoku table with the given size.
        /// </summary>
        void Draw(object o)
        {
            if (GetCurrentTableViewModel().AreAnyCellsFilled())
            {
                var messageBoxResult = MessageBoxService.Show(
                   Resources.MessageBox_DrawIfNumbersArePresented,
                   Resources.MessageBox_Question_Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.No)
                {
                    return;
                }
            }
            SudokuBoardSize sudokuBoardSize = (SudokuBoardSize)o;
            if (sudokuBoardSize.Height == 9 && sudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcClassicSudoku9x9Table();
            }
            else if (sudokuBoardSize.Height == 6 && sudokuBoardSize.Width == 6)
            {
                SudokuBoardControl = new UcClassicSudoku6x6Table();
            }
            else
            {
                SudokuBoardControl = new UcClassicSudoku4x4Table();
            }
            SolutionsCount = string.Empty;
        }

        /// <summary>
        /// Determines if solving the sudoku is possible.
        /// </summary>
        /// <returns>True if the solutions count is 0 or at least one cell is not filled in the table.</returns>
        bool CanSolve() => _solutions.Count == 0 
            || !GetCurrentTableViewModel().AreAllCellsFilled();

        /// <summary>
        /// Solves the classic sudoku.
        /// </summary>
        void Solve()
        {
            try
            {
                MessageBoxResult msgBoxResult = MessageBoxService.Show(
                    Resources.MessageBox_SolveSudoku,
                    Resources.MessageBox_Question_Title, MessageBoxButton.YesNo, MessageBoxImage.Question ,(Style)Application.Current.Resources["MessageBoxStyleForClassicSolve"]);

                if (msgBoxResult != MessageBoxResult.Cancel)
                {
                    _solutions.Clear();
                    SolutionsCount = string.Empty;
                    var board = CreateClassicBoard(((IClassicSudokuControl)SudokuBoardControl).BoardSize);

                    if (msgBoxResult == MessageBoxResult.Yes)
                    {

                        _sudokuSolverThread = new Thread(() =>
                        {
                            _solutions = (List<SudokuBoard>)Sudoku_SolverThread(board, true);
                            Action action = DisplayClassicSolutionAndMessage;
                            Application.Current.Dispatcher.Invoke(action);
                        });
                        _sudokuSolverThread.Start();
                    }
                    else
                    {
                        _sudokuSolverThread = new Thread(() =>
                        {
                            _solutions = (List<SudokuBoard>)Sudoku_SolverThread(board, false);
                            Action action = DisplayClassicSolutionAndMessage;
                            Application.Current.Dispatcher.Invoke(action);
                        });
                        _sudokuSolverThread.Start();
                    }
                    IsBusy = true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.Show(
                    $"{Resources.MessageBox_SolveSudokuError} {ex.Message}",
                    Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Question);
            }
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
            _saveFileDialog.Title = Resources.SaveDialog_Title_ClassicSudoku;
            _saveFileDialog.RestoreDirectory = true;
            _saveFileDialog.DefaultExt = "csu";
            _saveFileDialog.Filter = Resources.SaveLoadDialog_Filter_ClassicSudoku;
            _saveFileDialog.FilterIndex = 1;
            _saveFileDialog.CheckPathExists = true;
            _saveFileDialog.OverwritePrompt = true;

            if (_saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    var classicSudokuFile = new ClassicSudokuFile(CreateClassicBoard(SelectedSudokuBoardSize),
                        _solutions);

                    using (Stream stream = File.Open(_saveFileDialog.FileName, FileMode.Create))
                    {
                        var bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, classicSudokuFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"{Resources.MessageBox_Save_Error} {ex.Message}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
            _openFileDialog.Title = Resources.LoadDialog_Title_ClassicSudoku;
            _openFileDialog.RestoreDirectory = true;
            _openFileDialog.DefaultExt = "csu";
            _openFileDialog.Filter = Resources.SaveLoadDialog_Filter_ClassicSudoku;
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.CheckPathExists = true;
            _openFileDialog.CheckFileExists = true;

            if (_openFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    ClassicSudokuFile _classicSudokuFile = null;
                    using (Stream stream = File.Open(_openFileDialog.FileName, FileMode.Open))
                    {
                        var bformatter = new BinaryFormatter();
                        _classicSudokuFile = (ClassicSudokuFile)bformatter.Deserialize(stream);
                    }
                    SelectedSudokuBoardSize = _classicSudokuFile.Board.BoardSize;
                    Draw(SelectedSudokuBoardSize);
                    DisplayMatrixBoard(_classicSudokuFile.Board.OutputAsMatrix());
                    SolutionsCount = string.Empty;

                    _solutions = (List<SudokuBoard>)_classicSudokuFile.Solutions;
                    if (_solutions.Count > 1)
                    {
                        MessageBoxService.Show($"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part1} {_solutions.Count}). " +
                            $"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        SolutionsCount = $"{Resources.Main_SolutionsCounter}1/{ _solutions.Count}";
                        IsSolutionsCountVisible = true;
                    }
                    else if (_solutions.Count == 1)
                    {
                        MessageBoxService.Show(Resources.MessageBox_LoadedSudokuHasOneSolution, Resources.MessageBox_Information_Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        SolutionsCount = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"{Resources.MessageBox_Load_Error} {ex.Message}",
                        Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Determines if loading the previous solution is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanLoadPreviousSolution() => _solutions.Count > 1 && _solutionIndex > 0;

        /// <summary>
        /// Loads the next possible solution.
        /// </summary>
        void LoadPreviousSolution()
        {
            _solutionIndex -= 1;
            DisplayMatrixBoard(_solutions[_solutionIndex].OutputAsMatrix());
            SolutionsCount = $"{Resources.Main_SolutionsCounter} { _solutionIndex + 1 }/{ _solutions.Count }";
        }

        /// <summary>
        /// Determines if loading the next solution is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanLoadNextSolution() => _solutions.Count > 1 && _solutionIndex < _solutions.Count - 1;

        /// <summary>
        /// Loads the next possible solution.
        /// </summary>
        void LoadNextSolution()
        {
            _solutionIndex += 1;
            DisplayMatrixBoard(_solutions[_solutionIndex].OutputAsMatrix());
            SolutionsCount = $"{Resources.Main_SolutionsCounter} { _solutionIndex + 1 }/{ _solutions.Count }";
        }

        /// <summary>
        /// Determines if cancelling the busy task is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanCancelBusy() => true;

        /// <summary>
        /// Cancels the busy task.
        /// </summary>
        void CancelBusy()
        {
            var messageBoxResult = MessageBoxService.Show(Resources.MessageBox_AbortSolution, Resources.MessageBox_Warning_Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SudokuSolverThread.Abort();
                IsBusy = false;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the command properties in the viewmodel.
        /// </summary>
        void LoadCommands()
        {
            DrawClassicSudokuCommand = new ParameterizedCommand(Draw, CanDraw);
            SolveClassicSudokuCommand = new ParameterlessCommand(Solve, CanSolve);
            SaveClassicSudokuCommand = new ParameterlessCommand(Save, CanSave);
            LoadClassicSudokuCommand = new ParameterlessCommand(Load, CanLoad);
            LoadPreviousSolutionCommand = new ParameterlessCommand(LoadPreviousSolution, CanLoadPreviousSolution);
            LoadNextSolutionCommand = new ParameterlessCommand(LoadNextSolution, CanLoadNextSolution);
            CancelBusyCommand = new ParameterlessCommand(CancelBusy, CanCancelBusy);
        }

        /// <summary>
        /// Return the viewmodel of the current sudoku board control.
        /// </summary>
        /// <returns>IClassicSudokuTableViewModel</returns>
        IClassicSudokuTableViewModel GetCurrentTableViewModel() =>
            (IClassicSudokuTableViewModel)SudokuBoardControl.DataContext;

        /// <summary>
        /// Creates a classic Sudoku board, according to the given board size.
        /// </summary>
        /// <param name="sudokuBoardSize">Size information about the board.</param>
        /// <returns></returns>
        SudokuBoard CreateClassicBoard(SudokuBoardSize sudokuBoardSize)
        {
            SudokuBoard sudokuBoard;
            var boardControlViewModel = (IClassicSudokuTableViewModel)SudokuBoardControl.DataContext;
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
        /// Solver method for the sudoku solver thread.
        /// </summary>
        /// <param name="classicBoard">The classic Sudoku board what is need to be solved.</param>
        /// <param name="findAllSolutions">Determines if all possible solutions have to find or just one.</param>
        /// <returns>Enumerable of the solutions.</returns>
        IEnumerable<SudokuBoard> Sudoku_SolverThread(SudokuBoard classicBoard, bool findAllSolutions)
        {
            List<SudokuBoard> solutions = new List<SudokuBoard>();

            if (findAllSolutions)
            {
                solutions = classicBoard.Solve().ToList();
            }
            else
            {
                solutions.Add(classicBoard.SolveOnce());
            }

            return solutions;
        }

        /// <summary>
        /// Configures the viewmodel to display the classic solution(s) and a message about the possible solutions count.
        /// </summary>
        void DisplayClassicSolutionAndMessage()
        {
            IsBusy = false;
            if (_sudokuSolverThread.ThreadState != ThreadState.Aborted)
            {
                if (_solutions.Count > 0 && _solutions[0] != null)
                {
                    if (_solutions.Count > 1)
                    {
                        MessageBoxService.Show($"{Resources.MessageBox_SudokuHasMoreSolutions_Part1} { _solutions.Count}). {Resources.MessageBox_SudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                             MessageBoxButton.OK, MessageBoxImage.Information);
                        _solutionIndex = 0;
                        SolutionsCount = $"{Resources.Main_SolutionsCounter} { _solutionIndex + 1 }/{ _solutions.Count }";
                        IsSolutionsCountVisible = true;
                    }
                    else
                    {
                        MessageBoxService.Show(Resources.MessageBox_SudokuHasOneSolution, Resources.MessageBox_Information_Title,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    string[,] solution = _solutions[0].OutputAsMatrix();
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
        void DisplayMatrixBoard(string[,] board)
        {
            var boardControlViewModel = (IClassicSudokuTableViewModel)SudokuBoardControl.DataContext;
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