using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using Solvedoku.Classes;
using Solvedoku.Properties;
using Solvedoku.Views.ClassicSudoku;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    class ClassicSudokuViewModel : BaseSudokuViewModel
    {

        #region Properties

        public bool AreDiagonalRulesApplied
        {
            get => ((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).AreDiagonalRulesSet;
            set
            {
                ((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).AreDiagonalRulesSet = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SudokuBoardSize> SudokuBoardSizes => SudokuBoard.SudokuBoardSizes;

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
        /// Draws the classic sudoku table with the given size.
        /// </summary>
        protected override void Draw(object o)
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
            var areDiagonalRulesAlreadyApplied = AreDiagonalRulesApplied;
            if (sudokuBoardSize.Height == 9 && sudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcClassicSudoku9x9Table();
                _actualSudokuBoard = CreateBoard(sudokuBoardSize);
            }
            else if (sudokuBoardSize.Height == 6 && sudokuBoardSize.Width == 6)
            {
                SudokuBoardControl = new UcClassicSudoku6x6Table();
            }
            else
            {
                SudokuBoardControl = new UcClassicSudoku4x4Table();
            }
            ((BaseClassicSudokuTableViewModel)(SudokuBoardControl.DataContext)).AreDiagonalRulesSet = areDiagonalRulesAlreadyApplied;
            SolutionCounter = string.Empty;
            IsSolutionCounterVisible = false;
            _solutions.Clear();
        }

        /// <summary>
        /// Solves the classic sudoku.
        /// </summary>
        protected override void Solve()
        {
            try
            {
                var msgBoxResult = MessageBoxService.Show(
                    Resources.MessageBox_SolveSudoku,
                    Resources.MessageBox_Question_Title, MessageBoxButton.YesNo, MessageBoxImage.Question, (Style)Application.Current.Resources["MessageBoxStyleForSudokuSolve"]);

                if (msgBoxResult != MessageBoxResult.Cancel)
                {
                    _solutions.Clear();
                    SolutionCounter = string.Empty;
                    var board = CreateBoard(((IClassicSudokuControl)SudokuBoardControl).BoardSize, AreDiagonalRulesApplied);
                    if (msgBoxResult == MessageBoxResult.Yes)
                    {
                        _sudokuSolverThread = new Thread(() =>
                        {
                            int foundSolution = 0;
                            try
                            {
                                foreach (var item in Sudoku_SolverThread(board, true))
                                {
                                    if (item != null)
                                    {
                                        _solutions.Add(item);
                                        foundSolution++;
                                        FoundSolutionCounter = $"{Resources.TextBlock_FoundSolutions} {foundSolution}";
                                    }
                                }
                                Action action = DisplaySolutionAndMessage;
                                Application.Current.Dispatcher.Invoke(action);
                            }
                            catch (OutOfMemoryException)
                            {
                                _solutions.Clear();
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                MessageBoxService.Show($"{Resources.MessageBox_OutOfMemory}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error)));
                                IsBusy = false;
                            }

                        });
                        _sudokuSolverThread.Start();
                    }
                    else
                    {
                        _sudokuSolverThread = new Thread(() =>
                        {
                            if (Sudoku_SolverThread(board, false).First() != null)
                            {
                                _solutions.Add(Sudoku_SolverThread(board, false).First());
                            }
                            Action action = DisplaySolutionAndMessage;
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
                    Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Saves the classic sudoku to a file.
        /// </summary>
        protected override void Save()
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
                    var classicSudokuFile = new ClassicSudokuFile(CreateBoard(SelectedSudokuBoardSize, AreDiagonalRulesApplied),
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
        /// Loads a classic sudoku from a file.
        /// </summary>
        protected override void Load()
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
                    AreDiagonalRulesApplied = _classicSudokuFile.Board.HasDiagonalRules;
                    Draw(SelectedSudokuBoardSize);
                    DisplayMatrixBoard(_classicSudokuFile.Board.OutputAsStringMatrix());
                    SolutionCounter = string.Empty;

                    _solutions = _classicSudokuFile.Solutions;
                    if (_solutions.Count > 1)
                    {
                        MessageBoxService.Show($"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part1} {_solutions.Count}). " +
                            $"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        _solutionIndex = 0;
                        SolutionCounter = $"{ _solutionIndex + 1 }/{ _solutions.Count }";
                        IsSolutionCounterVisible = true;
                    }
                    else if (_solutions.Count == 1)
                    {
                        MessageBoxService.Show(Resources.MessageBox_LoadedSudokuHasOneSolution, Resources.MessageBox_Information_Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        SolutionCounter = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxService.Show($"{Resources.MessageBox_Load_Error} {ex.Message}",
                        Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}