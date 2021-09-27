using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Properties;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.ViewModels
{
    public abstract class BaseSudokuViewModel : ViewModelBase
    {
        #region Fields

        protected bool _isBusy;
        protected bool _isSolutionCounterVisible;
        protected bool _isSolvingCancelled;
        protected int _solutionIndex = -1;
        protected string _solutionCounter = string.Empty;
        protected string _foundSolutionCounter = string.Empty;
        protected Thread _sudokuSolverThread;
        protected Thread _sudokuSolverInspectorThread;
        protected Thread _sudokuSavingThread;
        protected Thread _sudokuLoadingThread;
        protected UserControl _sudokuBoardControl;
        protected UserControl _busyIndicatorContent;
        protected SudokuBoard _actualSudokuBoard;
        protected SudokuBoardSize _selectedSudokuBoardSize;
        protected SaveFileDialog _saveFileDialog = new SaveFileDialog();
        protected OpenFileDialog _openFileDialog = new OpenFileDialog();
        protected List<SudokuBoard> _solutions = new List<SudokuBoard>();
        private static ManualResetEvent mre = new ManualResetEvent(false);

        #endregion

        #region Properties

        public ICommand DrawSudokuCommand { get; set; }

        public ICommand SolveSudokuCommand { get; set; }

        public ICommand SaveSudokuCommand { get; set; }

        public ICommand LoadSudokuCommand { get; set; }

        public ICommand LoadPreviousSolutionCommand { get; set; }

        public ICommand LoadNextSolutionCommand { get; set; }

        public ICommand CancelBusySolvingCommand { get; set; }

        public ICommand CancelBusySavingCommand { get; set; }

        public ICommand CancelBusyLoadingCommand { get; set; }

        public ICommand BusyIndicatorLoadedCommand { get; set; }

        public Thread SudokuSolverThread
        {
            get => _sudokuSolverThread;
            set
            {
                _sudokuSolverThread = value;
            }
        }

        public Thread SudokuSolverInspectorThread 
        {
            get => _sudokuSolverInspectorThread;
            set
            {
                _sudokuSolverInspectorThread = value;
            }
        }

        public Thread SudokuSavingThread
        {
            get => _sudokuSavingThread;
            set
            {
                _sudokuSavingThread = value;
            }
        }

        public Thread SudokuLoadingThread
        {
            get => _sudokuLoadingThread;
            set
            {
                _sudokuLoadingThread = value;
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

        public UserControl BusyIndicatorContent
        {
            get => _busyIndicatorContent;
            set
            {
                _busyIndicatorContent = value;
                OnPropertyChanged();
            }
        }

        public SudokuBoard ActualSudokuBoard
        {
            get => _actualSudokuBoard;
            set => _actualSudokuBoard = value;      
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

        public List<SudokuBoard> Solutions => _solutions;

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

        #region Constructor
        public BaseSudokuViewModel():base(new MessageBoxService()){}
        public BaseSudokuViewModel(IMessageBoxService messageBoxService) : base(messageBoxService) { }
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
            DisplayMatrixBoard(_solutions[_solutionIndex].OutputAsStringMatrix());
            SolutionCounter = $"{ _solutionIndex + 1 }/{ _solutions.Count }";
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
            DisplayMatrixBoard(_solutions[_solutionIndex].OutputAsStringMatrix());
            SolutionCounter = $"{ _solutionIndex + 1 }/{ _solutions.Count }";
        }

        /// <summary>
        /// Determines if cancelling the busy solving is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanCancelBusySolving(object o) => true;

        /// <summary>
        /// Cancels the solving task.
        /// </summary>
        protected void CancelBusySolving(object o)
        {
            var messageBoxResult = MessageBoxService.Show(Resources.MessageBox_CancelSolving, Resources.MessageBox_Warning_Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _isSolvingCancelled = (bool)o;
                SudokuSolverThread.Abort();
                IsBusy = false;
            }
        }

        /// <summary>
        /// Determines if cancelling the saving task is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanCancelBusySaving() => true;

        /// <summary>
        /// Cancels the saving task.
        /// </summary>
        protected void CancelBusySaving()
        {
            var messageBoxResult = MessageBoxService.Show(Resources.MessageBox_CancelSaving, Resources.MessageBox_Warning_Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SudokuSavingThread.Abort();
                IsBusy = false;
            }
        }

        /// <summary>
        /// Determines if cancelling the saving task is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        protected bool CanCancelBusyLoading() => true;

        /// <summary>
        /// Cancels the saving task.
        /// </summary>
        protected void CancelBusyLoading()
        {
            var messageBoxResult = MessageBoxService.Show(Resources.MessageBox_CancelLoading, Resources.MessageBox_Warning_Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SudokuLoadingThread.Abort();
                IsBusy = false;
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Counts all possible solutions for the actual sudoku board.
        /// </summary>
        public void CountAllSolutions()
        {
            int foundSolution = 0;
            try
            {
                foreach (var item in Sudoku_SolverThread(_actualSudokuBoard, true))
                {
                    if (item != null)
                    {
                        lock (Solutions)
                        {
                            Solutions.Add(item);
                        }
                        foundSolution++;
                        FoundSolutionCounter = $"{Resources.TextBlock_FoundSolutions} {foundSolution}";
                    }
                }
                if (SudokuSolverThread != null)
                {
                    if (SudokuSolverThread.IsAlive)
                    {
                        SudokuSolverThread.Abort();
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                Solutions.Clear();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                MessageBoxService.Show($"{Resources.MessageBox_OutOfMemory}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error)));
                IsBusy = false;
            }
        }

        /// <summary>
        /// Counts one possible solution for the actual sudoku board.
        /// </summary>
        public void CountOneSolution()
        {
            if (Sudoku_SolverThread(_actualSudokuBoard, false).First() != null)
            {
                lock (Solutions)
                {
                    Solutions.Add(Sudoku_SolverThread(_actualSudokuBoard, false).First());
                }
            }
            if (SudokuSolverThread != null)
            {
                if (SudokuSolverThread.IsAlive)
                {
                    SudokuSolverThread.Abort();
                }
            }
        }

        public void InspectSolverThread()
        {
            while (true)
            {
                if (_sudokuSolverThread.ThreadState == ThreadState.Aborted || _sudokuSolverThread.ThreadState == ThreadState.Stopped)
                {
                    bool isSolvingCancelled = _sudokuSolverThread.ThreadState == ThreadState.AbortRequested && _isSolvingCancelled;
                    if (_isSolvingCancelled)
                    {
                        Solutions.Clear();
                    }
                    else
                    {
                        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            DisplaySolutionAndMessage();
                        }));
                    }
                    SudokuSolverInspectorThread.Abort();
                }
            }
        }

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
            CancelBusySolvingCommand = new ParameterizedCommand(CancelBusySolving, CanCancelBusySolving);
            CancelBusySavingCommand = new ParameterlessCommand(CancelBusySaving, CanCancelBusySaving);
            CancelBusyLoadingCommand = new ParameterlessCommand(CancelBusyLoading, CanCancelBusyLoading);
        }

        /// <summary>
        /// Returns the base viewmodel of the current sudoku board control.
        /// </summary>
        /// <returns>BaseSudokuTableViewModel</returns>
        public virtual BaseSudokuTableViewModel GetCurrentTableViewModel() =>
            (BaseSudokuTableViewModel)SudokuBoardControl?.DataContext;

        /// <summary>
        /// Creates a Sudoku board, according to the given board size.
        /// </summary>
        /// <param name="sudokuBoardSize">Size information about the board.</param>
        /// <returns>Sudoku board.</returns>
        public virtual SudokuBoard CreateBoard(SudokuBoardSize sudokuBoardSize, BaseSudokuTableViewModel baseSudokuTableViewModel, bool filledCellsAreBold, bool applyDiagonalRules = false)
        {
            SudokuBoard sudokuBoard;
            //var boardControlViewModel = (BaseSudokuTableViewModel)SudokuBoardControl.DataContext;
            if (sudokuBoardSize.BoxCountX == 3 && sudokuBoardSize.BoxCountY == 3)
            {
                sudokuBoard = SudokuFactory.ClassicWith3x3Boxes(applyDiagonalRules);
                for (int row = 0; row < sudokuBoardSize.Height; row++)
                {
                    string actRow = "";
                    for (int column = 0; column < sudokuBoardSize.Width; column++)
                    {
                        if (string.IsNullOrEmpty(baseSudokuTableViewModel.Cells[row][column]))
                        {
                            actRow += ".";
                            baseSudokuTableViewModel.BoldCells[row][column] = false;
                        }
                        else
                        {
                            baseSudokuTableViewModel.BoldCells[row][column] = filledCellsAreBold;
                            actRow += baseSudokuTableViewModel.Cells[row][column];
                        }
                    }
                    sudokuBoard.AddRow(actRow);
                }
            }
            else
            {
                sudokuBoard = SudokuFactory.SizeAndBoxes(sudokuBoardSize, applyDiagonalRules);
                for (int row = 0; row < sudokuBoardSize.Height; row++)
                {
                    string actRow = "";
                    for (int column = 0; column < sudokuBoardSize.Width; column++)
                    {
                        if (string.IsNullOrEmpty(baseSudokuTableViewModel.Cells[row][column]))
                        {
                            actRow += "0";
                            baseSudokuTableViewModel.BoldCells[row][column] = false;
                        }
                        else
                        {
                            baseSudokuTableViewModel.BoldCells[row][column] = true;
                            actRow += baseSudokuTableViewModel.Cells[row][column];
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
        public virtual SudokuBoard CreateBoard(SudokuBoardSize sudokuBoardSize, string[] areas, BaseSudokuTableViewModel baseSudokuTableViewModel, bool filledCellsAreBold)
        {
            SudokuBoard board = SudokuFactory.ClassicWithSpecialBoxes(areas);
            for (int row = 0; row < sudokuBoardSize.Height; row++)
            {
                string actRow = "";
                for (int column = 0; column < sudokuBoardSize.Width; column++)
                {
                    if (string.IsNullOrEmpty(baseSudokuTableViewModel.Cells[row][column]))
                    {
                        baseSudokuTableViewModel.BoldCells[row][column] = false;
                        actRow += ".";
                    }
                    else
                    {
                        baseSudokuTableViewModel.BoldCells[row][column] = filledCellsAreBold;
                        actRow += baseSudokuTableViewModel.Cells[row][column];
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
                    yield return item;
                }
            }
            else
            {
                SudokuBoard solvedBoard;
                try
                {
                    solvedBoard = sudokuBoard.Solve().First();
                }
                catch
                {
                    solvedBoard = null;
                }
                yield return solvedBoard;
                yield break;
            }
        }

        /// <summary>
        /// Configures the viewmodel to display the solution(s) and a message about the possible solutions count.
        /// </summary>
        public void DisplaySolutionAndMessage()
        {
            IsBusy = false;
            //if (!isSolvingCancelled)
            {
                if (_solutions.Count > 0 && _solutions[0] != null)
                {
                    _solutionIndex = 0;
                    if (_solutions.Count > 1)
                    {
                        SolutionCounter = $"{ _solutionIndex + 1 }/{ _solutions.Count }";
                        IsSolutionCounterVisible = true;
                        MessageBoxService.Show($"{Resources.MessageBox_SudokuHasMoreSolutions_Part1} { _solutions.Count}). {Resources.MessageBox_SudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                             MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        SolutionCounter = string.Empty;
                        IsSolutionCounterVisible = false;
                        MessageBoxService.Show(Resources.MessageBox_SudokuHasOneSolution, Resources.MessageBox_Information_Title,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    string[,] solution = _solutions[_solutionIndex].OutputAsStringMatrix();
                    DisplayMatrixBoard(solution);
                }
                else
                {
                    SolutionCounter = string.Empty;
                    IsSolutionCounterVisible = false;
                    MessageBoxService.Show(Resources.MessageBox_SudokuHasNoSolution, Resources.MessageBox_Information_Title,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        /// <summary>
        /// Loads the value from the given matrix into the selected table's viewmodel.
        /// </summary>
        /// <param name="board"></param>
        public void DisplayMatrixBoard(string[,] board)
        {
            var boardControlViewModel = GetCurrentTableViewModel();
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