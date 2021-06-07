﻿using Microsoft.Win32;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Views.JigsawSudoku;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using Solvedoku.Properties;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    class JigsawSudokuViewModel : ViewModelBase
    {
        #region Fields
        static JigsawSudokuViewModel _instance;
        bool _isBusy;
        bool _isSolutionsCountVisible;
        int _solutionIndex = -1;
        string _solutionsCount = string.Empty;
        Thread _sudokuSolverThread;
        Color? _selectedColor;
        UserControl _sudokuBoardControl;
        SudokuBoardSize _selectedSudokuBoardSize = new SudokuBoardSize { BoxCountX = 3, BoxCountY = 3, Height = 9, Width = 9 };
        SaveFileDialog _saveFileDialog = new SaveFileDialog();
        OpenFileDialog _openFileDialog = new OpenFileDialog();
        List<SudokuBoard> _solutions = new List<SudokuBoard>();
        #endregion

        #region Properties

        public static JigsawSudokuViewModel Instance { get => _instance; }

        public ICommand DrawJigsawSudokuCommand { get; set; }

        public ICommand SolveJigsawSudokuCommand { get; set; }

        public ICommand SaveJigsawSudokuCommand { get; set; }

        public ICommand LoadJigsawSudokuCommand { get; set; }

        public ICommand LoadPreviousSolutionCommand { get; set; }

        public ICommand LoadNextSolutionCommand { get; set; }

        public ICommand BusyIndicatorLoadedCommand { get; set; }

        public Thread SudokuSolverThread { get => _sudokuSolverThread; }

        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
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

        public ObservableCollection<ColorItem> PuzzleColors => SudokuBoard.PuzzleColors;

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

        public ICommand CancelBusyCommand { get; set; }
        #endregion

        #region Constructor

        public JigsawSudokuViewModel()
        {
            LoadCommands();
            SelectedColor = Colors.LightBlue;
            SudokuBoardControl = new UcJigsawSudoku9x9Table();
            _instance = this;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Determines if drawing a sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanDraw(object o) => true;

        /// <summary>
        /// Draws the sudoku table with the given size.
        /// </summary>
        void Draw(object o)
        {
            SudokuBoardSize sudokuBoardSize = (SudokuBoardSize)o;
            if (sudokuBoardSize.Height == 9 && sudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcJigsawSudoku9x9Table();
            }
            SolutionsCount = string.Empty;
        }

        /// <summary>
        /// Determines if solving the sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanSolve() => true;

        /// <summary>
        /// Solves the sudoku.
        /// </summary>
        void Solve()
        {
            try
            {
                var messageBoxResult = MessageBoxService.Show(Resources.MessageBox_SolveSudoku,
                    Resources.MessageBox_Question_Title,
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult != MessageBoxResult.Cancel)
                {
                    SolutionsCount = string.Empty;

                    string[] areas = GetCurrentTableViewModel().GetJigsawAreasAsArray();
                    var board = CreateJigsawBoard(((IJigsawSudokuControl)SudokuBoardControl).BoardSize, areas);

                    _solutionIndex = 0;

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        _sudokuSolverThread = new Thread(() =>
                        {
                            _solutions = (List<SudokuBoard>)Sudoku_SolverThread(board, true);
                            Action action = DisplaySolutionAndMessage;
                            Application.Current.Dispatcher.Invoke(action);
                        });
                        _sudokuSolverThread.Start();
                    }
                    else
                    {
                        _sudokuSolverThread = new Thread(() =>
                        {
                            _solutions = (List<SudokuBoard>)Sudoku_SolverThread(board, true);
                            Action action = DisplaySolutionAndMessage;
                            Application.Current.Dispatcher.Invoke(action);
                        });
                        _sudokuSolverThread.Start();
                    }
                    IsBusy = true;
                }
            }
            catch
            {
                MessageBoxService.Show(Resources.MessageBox_SolveSudokuError,
                    Resources.MessageBox_Error_Title,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Determines if saving the sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanSave() => true;

        /// <summary>
        /// Saves the sudoku to a file.
        /// </summary>
        void Save()
        {
            _saveFileDialog.Title = Resources.SaveDialog_Title_JigsawSudoku;
            _saveFileDialog.RestoreDirectory = true;
            _saveFileDialog.DefaultExt = "jsu";
            _saveFileDialog.Filter = Resources.SaveLoadDialog_Filter_JigsawSudoku;
            _saveFileDialog.FilterIndex = 1;
            _saveFileDialog.CheckPathExists = true;
            _saveFileDialog.OverwritePrompt = true;

            if (_saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    var jigsawSudokuFile = new JigsawSudokuFile(CreateJigsawBoard(SelectedSudokuBoardSize,
                        GetCurrentTableViewModel().GetJigsawAreasAsArray()),
                        ((IJigsawSudokuControl)SudokuBoardControl).BoardSize,
                        GetCurrentTableViewModel().GetJigsawAreasAsMatrix(),
                        _solutions);

                    using (Stream stream = File.Open(_saveFileDialog.FileName, FileMode.Create))
                    {
                        var bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, jigsawSudokuFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"{Resources.MessageBox_Save_Error} {ex.Message}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Determines if loading a sudoku is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanLoad() => true;

        /// <summary>
        /// Loads a sudoku from a file.
        /// </summary>
        void Load()
        {
            _openFileDialog.Title = Resources.LoadDialog_Title_JigsawSudoku;
            _openFileDialog.RestoreDirectory = true;
            _openFileDialog.DefaultExt = "jsu";
            _openFileDialog.Filter = Resources.SaveLoadDialog_Filter_JigsawSudoku;
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.CheckPathExists = true;
            _openFileDialog.CheckFileExists = true;

            if (_openFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    JigsawSudokuFile _jigsawSudokuFile = null;
                    using (Stream stream = File.Open(_openFileDialog.FileName, FileMode.Open))
                    {
                        var bformatter = new BinaryFormatter();
                        _jigsawSudokuFile = (JigsawSudokuFile)bformatter.Deserialize(stream);
                    }
                    SelectedSudokuBoardSize = _jigsawSudokuFile.Board.BoardSize;
                    Draw(SelectedSudokuBoardSize);
                    DisplayAreas(_jigsawSudokuFile.Areas);
                    DisplayMatrixBoard(_jigsawSudokuFile.Board.OutputAsMatrix());
                    SolutionsCount = string.Empty;

                    _solutions = _jigsawSudokuFile.Solutions;
                    if (_solutions.Count > 1)
                    {
                        MessageBoxService.Show($"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part1} {_solutions.Count}). " +
                            $"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        SolutionsCount = $"{Resources.Main_SolutionsCounter}{ _solutions.Count}";
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
            SolutionsCount = $"Megoldások: { _solutionIndex + 1 }/{ _solutions.Count }";
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
            SolutionsCount = $"Megoldások: { _solutionIndex + 1 }/{ _solutions.Count }";
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the command properties in the viewmodel.
        /// </summary>
        private void LoadCommands()
        {
            DrawJigsawSudokuCommand = new ParameterizedCommand(Draw, CanDraw);
            SolveJigsawSudokuCommand = new ParameterlessCommand(Solve, CanSolve);
            SaveJigsawSudokuCommand = new ParameterlessCommand(Save, CanSave);
            LoadJigsawSudokuCommand = new ParameterlessCommand(Load, CanLoad);
            LoadPreviousSolutionCommand = new ParameterlessCommand(LoadPreviousSolution, CanLoadPreviousSolution);
            LoadNextSolutionCommand = new ParameterlessCommand(LoadNextSolution, CanLoadNextSolution);
        }

        /// <summary>
        /// Returns the viewmodel of the current sudoku board control.
        /// </summary>
        /// <returns>IJigsawSudokuTableViewModel</returns>
        IJigsawSudokuTableViewModel GetCurrentTableViewModel() =>
            (IJigsawSudokuTableViewModel)SudokuBoardControl.DataContext;

        /// <summary>
        /// Creates a jigsaw Sudoku board, according to the given board size.
        /// </summary>
        /// <param name="sudokuBoardSize">Size information about the board.</param>
        /// <returns></returns>
        private SudokuBoard CreateJigsawBoard(SudokuBoardSize sudokuBoardSize, string[] areas)
        {
            SudokuBoard board = SudokuFactory.ClassicWithSpecialBoxes(areas);
            var boardControlViewModel = (IJigsawSudokuTableViewModel)SudokuBoardControl.DataContext;
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
                        // actCell.FontWeight = FontWeights.Bold;
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
        /// <param name="classicBoard">The classic Sudoku board what is need to be solved.</param>
        /// <param name="findAllSolutions">Determines if all possible solutions have to find or just one.</param>
        /// <returns>Enumerable of the solutions.</returns>
        IEnumerable<SudokuBoard> Sudoku_SolverThread(SudokuBoard sudokuBoard, bool findAllSolutions)
        {
            IsBusy = false;
            List<SudokuBoard> solutions = new List<SudokuBoard>();

            if (findAllSolutions)
            {
                solutions = sudokuBoard.Solve().ToList();
            }
            else
            {
                solutions.Add(sudokuBoard.SolveOnce());
            }

            return solutions;
        }

        /// <summary>
        /// Configures the viewmodel to display the solution(s) and a message about the possible solutions count.
        /// </summary>
        private void DisplaySolutionAndMessage()
        {
            IsBusy = false;
            if (_sudokuSolverThread.ThreadState != ThreadState.Aborted)
            {
                if (_solutions.Count > 0 && _solutions[0] != null)
                {
                    if (_solutions.Count > 1)
                    {
                        MessageBoxService.Show($"A puzzle feladványnak több megoldása is van (összesen { _solutions.Count })." +
                            $" A táblázat alatt található nyilakkal tudsz köztük váltani.", "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        _solutionIndex = 0;
                        SolutionsCount = $"Megoldások: { _solutionIndex + 1 }/{ _solutions.Count }";

                    }
                    else
                    {
                        MessageBoxService.Show("A puzzle feladványnak egy megoldása van.",
                            "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    string[,] actualSolution = _solutions[0].OutputAsMatrix();
                    DisplayMatrixBoard(actualSolution);
                }
                else
                {
                    MessageBoxService.Show("A puzzle feladványnak sajnos nincs megoldása.",
                        "Információ!",
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
            var boardControlViewModel = (IJigsawSudokuTableViewModel)SudokuBoardControl.DataContext;
            for (int column = 0; column < board.GetLength(0); column++)
            {
                for (int row = 0; row < board.GetLength(1); row++)
                {
                    if (board[column, row] != "0")
                    {
                        boardControlViewModel.Cells[column][row] = board[column, row];
                    }
                }
            }
        }

        void DisplayAreas(int[,] areas)
        {
            var boardControlViewModel = (IJigsawSudokuTableViewModel)SudokuBoardControl.DataContext;
            for (int column = 0; column < areas.GetLength(0); column++)
            {
                for (int row = 0; row < areas.GetLength(1); row++)
                {
                    //if (areas[column, row] != -1)
                    {
                        boardControlViewModel.JigsawAreas[column][row] = areas[column, row];
                    }
                }
            }
        }

        /*private string[] GetPuzzleAreas()
        {
            Grid puzzleGrid = (Grid)LogicalTreeHelper.FindLogicalNode(PuzzleDockPanel, "PuzzleGrid");
            List<string> colorInfos = new List<string>();
            List<SolidColorBrush> puzzleBrushes = new List<SolidColorBrush>();
            foreach (ColorItem cItem in PuzzleColorPicker.StandardColors)
            {
                puzzleBrushes.Add(new SolidColorBrush(cItem.Color.GetValueOrDefault()));
            }

            for (int row = 0; row < puzzleGrid.RowDefinitions.Count; row++)
            {
                string actRow = "";
                for (int column = 0; column < puzzleGrid.ColumnDefinitions.Count; column++)
                {

                    TextBox actCell = (TextBox)this.FindName("TbPuzzleCell" + column + row);
                    int index = -1;
                    for (int i = 0; i < puzzleBrushes.Count; i++)
                    {
                        if (puzzleBrushes[i].Color == ((SolidColorBrush)actCell.Background).Color)
                        {
                            index = i;
                        }
                    }
                    if (index != -1)
                    {
                        actRow += index;
                    }


                }
                colorInfos.Add(actRow);
            }

            return colorInfos.ToArray();
        }*/
        #endregion
        /*  public bool IsBusy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
          public Thread SudokuSolverThread => throw new NotImplementedException();*/
    }
}