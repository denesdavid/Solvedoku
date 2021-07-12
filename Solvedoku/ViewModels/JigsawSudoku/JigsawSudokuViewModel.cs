using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using Solvedoku.Classes;
using Solvedoku.Views.JigsawSudoku;
using Solvedoku.Properties;

namespace Solvedoku.ViewModels.JigsawSudoku
{
    [Serializable]
    class JigsawSudokuViewModel : BaseSudokuViewModel
    {
        #region Fields

        static JigsawSudokuViewModel _instance;
        Color? _selectedColor;
        
        #endregion

        #region Properties

        public static JigsawSudokuViewModel Instance { get => _instance; }

        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ColorItem> JigsawColors => new ObservableCollection<ColorItem> {
                new ColorItem(Colors.LightBlue, Resources.Color_LightBlue),
                new ColorItem(Colors.CornflowerBlue, Resources.Color_CornflowerBlue),
                new ColorItem(Colors.Magenta, Resources.Color_Magenta),
                new ColorItem(Colors.Red, Resources.Color_Red),
                new ColorItem(Colors.Green, Resources.Color_Green),
                new ColorItem(Colors.Yellow, Resources.Color_Yellow),
                new ColorItem(Colors.RosyBrown, Resources.Color_RosyBrown),
                new ColorItem(Colors.Orange, Resources.Color_Orange),
                new ColorItem(Colors.MediumPurple, Resources.Color_MediumPurple),
                new ColorItem(Colors.LightGray, Resources.Color_LightGray) };

        #endregion

        #region Constructor

        public JigsawSudokuViewModel()
        {
            LoadCommands();
            SelectedColor = Colors.LightBlue;
            SudokuBoardControl = new UcJigsawSudoku9x9Table();
            _selectedSudokuBoardSize = new SudokuBoardSize { BoxCountX = 3, BoxCountY = 3, Height = 9, Width = 9 };
            _instance = this;
        }

        public JigsawSudokuViewModel(JigsawSudokuViewModel oldViewModel)
        {
            LoadCommands();
            SelectedColor = oldViewModel.SelectedColor;
            SudokuBoardControl = oldViewModel.SudokuBoardControl;
            SolutionCounter = oldViewModel.SolutionCounter;
            _selectedSudokuBoardSize = new SudokuBoardSize { BoxCountX = 3, BoxCountY = 3, Height = 9, Width = 9 };
            _instance = this;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Draws the jigsaw sudoku table with the given size.
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
            if (sudokuBoardSize.Height == 9 && sudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcJigsawSudoku9x9Table();
            }
            _solutions.Clear();
            SolutionCounter = string.Empty;
            IsSolutionCounterVisible = false;
        }

        /// <summary>
        /// Solves the jigsaw sudoku.
        /// </summary>
        protected override void Solve()
        {
            try
            {
                var messageBoxResult = MessageBoxService.Show(
                    Resources.MessageBox_SolveSudoku,
                    Resources.MessageBox_Question_Title, MessageBoxButton.YesNo, MessageBoxImage.Question, (Style)Application.Current.Resources["MessageBoxStyleForSudokuSolve"]);

                if (messageBoxResult != MessageBoxResult.Cancel)
                {
                    SolutionCounter = string.Empty;

                    string[] areas = ((BaseJigsawSudokuTableViewModel)GetCurrentTableViewModel()).GetJigsawAreasAsArray();
                    var board = CreateBoard(((IJigsawSudokuControl)SudokuBoardControl).BoardSize, areas);

                    _solutionIndex = 0;

                    if (messageBoxResult == MessageBoxResult.Yes)
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
            catch
            {
                MessageBoxService.Show(Resources.MessageBox_SolveSudokuError,
                    Resources.MessageBox_Error_Title,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Saves the jigsaw sudoku to a file.
        /// </summary>
        protected override void Save()
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
                    var jigsawSudokuFile = new JigsawSudokuFile(CreateBoard(SelectedSudokuBoardSize,
                        ((BaseJigsawSudokuTableViewModel)GetCurrentTableViewModel()).GetJigsawAreasAsArray()),
                        ((IJigsawSudokuControl)SudokuBoardControl).BoardSize,
                        ((BaseJigsawSudokuTableViewModel)GetCurrentTableViewModel()).GetJigsawAreasAsMatrix(),
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
        /// Loads a jigsaw sudoku from a file.
        /// </summary>
        protected override void Load()
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
                    DisplayMatrixBoard(_jigsawSudokuFile.Board.OutputAsStringMatrix());
                    SolutionCounter = string.Empty;

                    _solutions = _jigsawSudokuFile.Solutions;
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

        #region Methods

        void DisplayAreas(int[,] areas)
        {
            var boardControlViewModel = (BaseJigsawSudokuTableViewModel)SudokuBoardControl.DataContext;
            for (int column = 0; column < areas.GetLength(0); column++)
            {
                for (int row = 0; row < areas.GetLength(1); row++)
                {
                    {
                        boardControlViewModel.JigsawAreas[column][row] = areas[column, row];
                    }
                }
            }
        }

        public JigsawSudokuViewModel Clone()
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, this);

            ms.Position = 0;
            object obj = bf.Deserialize(ms);
            ms.Close();

            return obj as JigsawSudokuViewModel;
        }

        #endregion
    }
}