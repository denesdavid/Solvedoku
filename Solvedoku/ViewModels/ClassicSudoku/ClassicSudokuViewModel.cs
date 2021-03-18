using Microsoft.Win32;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Services.MessageBox;
using Solvedoku.Views.ClassicSudoku;
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

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudokuViewModel : ViewModelBase
    {
        #region Fields
        bool _isBusy;
        bool _isNextSolutionEnabled;
        bool _isPreviousSolutionEnabled;
        string _solutionsCount = string.Empty;
        Thread _sudokuSolverThread;
        UserControl _sudokuBoardControl;
        SudokuBoard _sudokuBoard;
        SudokuBoardSize _selectedSudokuBoardSize;
        SaveFileDialog _saveFileDialog = new SaveFileDialog();
        OpenFileDialog _openFileDialog = new OpenFileDialog();
        List<SudokuBoard> _classicSolutions = new List<SudokuBoard>();
        #endregion

        #region Properties

        public ICommand DrawClassicSudokuCommand { get; set; }

        public ICommand SolveClassicSudokuCommand { get; set; }

        public ICommand SaveClassicSudokuCommand { get; set; }

        public ICommand LoadClassicSudokuCommand { get; set; }

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

        public bool IsNextSolutionEnabled 
        {
            get => _isNextSolutionEnabled;
            set
            {
                _isNextSolutionEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsPreviousSolutionEnabled 
        {
            get => _isPreviousSolutionEnabled;
            set
            {
                _isPreviousSolutionEnabled = value;
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
        bool CanDraw() => true;

        /// <summary>
        /// Draws the classic sudoku table with the given size.
        /// </summary>
        void Draw()
        {
            if (SelectedSudokuBoardSize.Height == 9 && SelectedSudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcClassicSudoku9x9Table();
            }
            else if (SelectedSudokuBoardSize.Height == 6 && SelectedSudokuBoardSize.Width == 6)
            {
                SudokuBoardControl = new UcClassicSudoku6x6Table();
            }
            else
            {
                SudokuBoardControl = new UcClassicSudoku4x4Table();
            }
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
            try
            {
                MessageBoxResult msgBoxResult = MessageBoxService.Show(
                    "Kérlek válaszd ki, hogy az összes megoldást (feltéve ha van egynél több, illetve ez időigényes is lehet), vagy csak egy lehetségeset szeretnél megkapni.",
                    "Kérdés", MessageBoxButton.YesNo, MessageBoxImage.Question ,(Style)Application.Current.Resources["MessageBoxStyleForClassicSolve"]);

                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    IsNextSolutionEnabled = false;
                    IsPreviousSolutionEnabled = false;
                    _classicSolutions.Clear();
                    SolutionsCount = string.Empty;
                    var board = CreateClassicBoard(((IClassicSudokuControl)SudokuBoardControl).BoardSize);

                    if (msgBoxResult == MessageBoxResult.Yes)
                    {

                        _sudokuSolverThread = new Thread(() =>
                        {
                            _classicSolutions = Sudoku_SolverThread(board, true);
                            /*Action action = DisplayClassicSolutionAndMessage;
                            Dispatcher.BeginInvoke(action);*/
                        });
                        _sudokuSolverThread.Start();
                    }
                    else
                    {
                        _sudokuSolverThread = new Thread(() =>
                        {
                            _classicSolutions = Sudoku_SolverThread(board, false);
                            /*Action action = DisplayClassicSolutionAndMessage;
                            Dispatcher.BeginInvoke(action);*/
                        });
                        _sudokuSolverThread.Start();

                    }
                    IsBusy = true;
                }
            }
            catch
            {
                MessageBoxService.Show(
                    "A klasszikus feladvány megoldása során hiba lépett fel. Kérlek ellenőrizd, hogy helyesen adtad-e meg a feladatot.",
                    "Hiba!", MessageBoxButton.OK, MessageBoxImage.Question);
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
            _saveFileDialog.Title = "Klasszikus SUDOKU mentése..";
            _saveFileDialog.RestoreDirectory = true;
            _saveFileDialog.DefaultExt = "csu";
            _saveFileDialog.Filter = "Klasszikus SUDOKU fájlok (*.csu)|*.csu";
            _saveFileDialog.FilterIndex = 1;
            _saveFileDialog.CheckPathExists = true;
            _saveFileDialog.OverwritePrompt = true;

            if (_saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    var classicSudokuFile = new ClassicSudokuFile();
                    classicSudokuFile.Board = _sudokuBoard;
                    //classicSudokuFile.Solutions = _classicSolutions;
                    classicSudokuFile.BoardSize = SelectedSudokuBoardSize;

                    using (Stream stream = File.Open(_saveFileDialog.FileName, FileMode.Create))
                    {
                        var bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, classicSudokuFile);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"A Sudoku mentése során hiba lépett fel. {ex.Message}", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            _saveFileDialog.Reset();
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
            DrawClassicSudokuCommand = new ParameterlessCommand(Draw, CanDraw);
            SolveClassicSudokuCommand = new ParameterlessCommand(Solve, CanSolve);
            SaveClassicSudokuCommand = new ParameterlessCommand(Save, CanSave);
            LoadClassicSudokuCommand = new ParameterlessCommand(Load, CanLoad);
        }

        /// <summary>
        /// Creates a classic Sudoku board, according to the given board size.
        /// </summary>
        /// <param name="sudokuBoardSize">Size information about the board.</param>
        /// <returns></returns>
        SudokuBoard CreateClassicBoard(SudokuBoardSize sudokuBoardSize)
        {
            var sudokuBoard = SudokuFactory.ClassicWith3x3Boxes();
            var boardControlViewModel = (ClassicSudoku9x9TableViewModel)SudokuBoardControl.DataContext;
            if (sudokuBoardSize.BoxCountX == 3 && sudokuBoardSize.BoxCountY == 3)
            {
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
                    _sudokuBoard.AddRow(actRow);
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
                            //actCell.FontWeight = FontWeights.Bold;
                            actRow += boardControlViewModel.Cells[row][column];
                        }
                    }
                    _sudokuBoard.AddRow(actRow);
                }
            }
            return sudokuBoard;
            #endregion
        }

        List<SudokuBoard> Sudoku_SolverThread(SudokuBoard classicBoard, bool findAllSolutions)
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

        private void DisplayClassicSolutionAndMessage()
        {
            IsBusy = false;
            if (_sudokuSolverThread.ThreadState != ThreadState.Aborted)
            {
                if (_classicSolutions.Count > 0 && _classicSolutions[0] != null)
                {
                    if (_classicSolutions.Count > 1)
                    {
                        MessageBoxService.Show("A klasszikus feladványnak több megoldása is van (összesen " + _classicSolutions.Count + "). A táblázat alatt található nyilakkal tudsz köztük váltani.", "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        IsNextSolutionEnabled = true;
                        SolutionsCount = "Megoldások: 1/" + _classicSolutions.Count;
                        //LbClassicSolvesCount.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBoxService.Show("A klasszikus feladványnak egy megoldása van.", "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    string[,] actSolution = _classicSolutions[0].OutputAsMatrix();
                    DisplayBoard(actSolution, "TbClassicCell");

                }
                else
                {
                    MessageBoxService.Show("A klasszikus feladványnak sajnos nincs megoldása.", "Információ!",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DisplaySolution(string[,] board)
        {
            var boardControlViewModel = (ClassicSudoku9x9TableViewModel)SudokuBoardControl.DataContext;
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
    }
}