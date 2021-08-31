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
using System.Windows.Threading;
using Solvedoku.Views.BusyIndicatorContent;

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
            Draw(SudokuBoard.SudokuBoardSizes[0]);
            _selectedSudokuBoardSize = SudokuBoard.SudokuBoardSizes[0];
            _instance = this;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Draws the jigsaw sudoku table with the given size.
        /// </summary>
        protected override void Draw(object o)
        {
            if (GetCurrentTableViewModel() != null && GetCurrentTableViewModel().AreAnyCellsFilled())
            {
                var messageBoxResult = MessageBoxService.Show(
                   Resources.MessageBox_DrawIfNumbersArePresented,
                   Resources.MessageBox_Question_Title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.No || messageBoxResult == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            SudokuBoardSize sudokuBoardSize = (SudokuBoardSize)o;
            if (sudokuBoardSize.Height == 9 && sudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcJigsawSudoku9x9Table();
                _actualSudokuBoard = CreateBoard(sudokuBoardSize, new string[9]
                { 
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1",
                    "-1-1-1-1-1-1-1-1-1", 
                    "-1-1-1-1-1-1-1-1-1",
                }, (BaseSudokuTableViewModel)SudokuBoardControl.DataContext, false);
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
                    _solutions.Clear();
                    SolutionCounter = string.Empty;
                    string[] areas = ((BaseJigsawSudokuTableViewModel)GetCurrentTableViewModel()).GetJigsawAreasAsArray();
                    _actualSudokuBoard = CreateBoard(((IJigsawSudokuControl)SudokuBoardControl).BoardSize, areas, (BaseSudokuTableViewModel)SudokuBoardControl.DataContext, true);
                    _solutionIndex = 0;

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        BusyIndicatorContent = new UcSolvingBusyIndicatorContent();
                        _sudokuSolverThread = new Thread(() =>
                        {
                            int foundSolution = 0;
                            try
                            {
                                foreach (var item in Sudoku_SolverThread(_actualSudokuBoard, true))
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
                            if (Sudoku_SolverThread(_actualSudokuBoard, false).First() != null)
                            {
                                _solutions.Add(Sudoku_SolverThread(_actualSudokuBoard, false).First());
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

        protected void CountAllSolutions()
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
                Action action = DisplaySolutionAndMessage;
                Application.Current.Dispatcher.Invoke(action);
            }
            catch (OutOfMemoryException)
            {
                Solutions.Clear();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                MessageBoxService.Show($"{Resources.MessageBox_OutOfMemory}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error)));
                IsBusy = false;
            }
        }

        protected void CountOneSolution()
        {
            if (Sudoku_SolverThread(_actualSudokuBoard, false).First() != null)
            {
                lock (Solutions)
                {
                    Solutions.Add(Sudoku_SolverThread(_actualSudokuBoard, false).First());
                }
            }
            Action action = DisplaySolutionAndMessage;
            Application.Current.Dispatcher.Invoke(action);
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
                    BusyIndicatorContent = new UcSavingBusyIndicatorContent();
                    var jigsawSudokuFile = new JigsawSudokuFile();
                    jigsawSudokuFile.SelectedSudokuBoardSize = SelectedSudokuBoardSize;
                    jigsawSudokuFile.Areas = ((BaseJigsawSudokuTableViewModel)SudokuBoardControl.DataContext).GetJigsawAreasAsMatrix();
                    jigsawSudokuFile.Cells = new ObservableCollection<ObservableCollection<string>>(((BaseJigsawSudokuTableViewModel)SudokuBoardControl.DataContext).Cells);
                    jigsawSudokuFile.BoldCells = new ObservableCollection<ObservableCollection<bool>>(((BaseJigsawSudokuTableViewModel)SudokuBoardControl.DataContext).BoldCells);
                    jigsawSudokuFile.Solutions = _solutions;
                    jigsawSudokuFile.SolutionIndex = _solutionIndex;
                    jigsawSudokuFile.SolutionCounter = SolutionCounter;
                    jigsawSudokuFile.IsSolutionCounterVisible = IsSolutionCounterVisible;

                    _sudokuSavingThread = new Thread(() =>
                    {
                        try
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                            new Action(() => SerializeJigsawSudokuFile(_saveFileDialog.FileName, jigsawSudokuFile)));
                        }
                        catch (OutOfMemoryException)
                        {
                            _solutions.Clear();
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            MessageBoxService.Show($"{Resources.MessageBox_OutOfMemory}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error)));
                            IsBusy = false;
                        }

                    });
                    _sudokuSavingThread.Start();
                    IsBusy = true;
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
                    BusyIndicatorContent = new UcLoadingBusyIndicatorContent();
                    _sudokuLoadingThread = new Thread(() =>
                    {
                        try
                        {
                            JigsawSudokuFile jigsawSudokuFile = null;
                            using (Stream stream = File.Open(_openFileDialog.FileName, FileMode.Open))
                            {
                                var bformatter = new BinaryFormatter();
                                jigsawSudokuFile = (JigsawSudokuFile)bformatter.Deserialize(stream);
                            }
                            Application.Current.Dispatcher.Invoke(new Action(() => DeserializeJigsawSudokuFile(jigsawSudokuFile)));
                        }
                        catch (OutOfMemoryException)
                        {
                            _solutions.Clear();
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            MessageBoxService.Show($"{Resources.MessageBox_OutOfMemory}", Resources.MessageBox_Error_Title, MessageBoxButton.OK, MessageBoxImage.Error)));
                            IsBusy = false;
                        }

                    });
                    _sudokuLoadingThread.Start();
                    IsBusy = true;
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

        /// <summary>
        /// Serializes the given jigsaw sudoku file into the given file.
        /// </summary>
        /// <param name="filePath">File where the jigsaw sudoku file will be serialized.</param>
        /// <param name="jigsawSudokuFile">Jigsaw sudoku file.</param>
        void SerializeJigsawSudokuFile(string filePath, JigsawSudokuFile jigsawSudokuFile)
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, jigsawSudokuFile);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Loads the deserialized jigsaw sudoku file.
        /// </summary>
        /// <param name="jigsawSudokuFile">Jigsaw sudoku file</param>
        void DeserializeJigsawSudokuFile(JigsawSudokuFile jigsawSudokuFile)
        {
            IsBusy = false;
            SelectedSudokuBoardSize = jigsawSudokuFile.SelectedSudokuBoardSize;
            Draw(SelectedSudokuBoardSize);
            DisplayAreas(jigsawSudokuFile.Areas);
            ((BaseJigsawSudokuTableViewModel)SudokuBoardControl.DataContext).Cells = jigsawSudokuFile.Cells;
            ((BaseJigsawSudokuTableViewModel)SudokuBoardControl.DataContext).BoldCells = jigsawSudokuFile.BoldCells;
            SolutionCounter = string.Empty;
            _solutions = jigsawSudokuFile.Solutions;
            _solutionIndex = jigsawSudokuFile.SolutionIndex;
            SolutionCounter = jigsawSudokuFile.SolutionCounter;
            IsSolutionCounterVisible = jigsawSudokuFile.IsSolutionCounterVisible;

            if (_solutions.Count > 1)
            {
                MessageBoxService.Show($"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part1} {_solutions.Count}). " +
                   $"{Resources.MessageBox_LoadedSudokuHasMoreSolutions_Part2}", Resources.MessageBox_Information_Title,
                   MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (_solutions.Count == 1)
            {
                MessageBoxService.Show(Resources.MessageBox_LoadedSudokuHasOneSolution, Resources.MessageBox_Information_Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Loads the jigsaw areas into the curent sudoku board viewmodel.
        /// </summary>
        /// <param name="areas">Integer array of the jigsaw areas.</param>
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

        #endregion
    }
}