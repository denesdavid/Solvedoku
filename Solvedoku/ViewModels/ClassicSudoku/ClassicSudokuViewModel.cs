using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Solvedoku.Classes;
using Solvedoku.Properties;
using Solvedoku.Services.MessageBox;
using Solvedoku.Views.BusyIndicatorContent;
using Solvedoku.Views.ClassicSudoku;

namespace Solvedoku.ViewModels.ClassicSudoku
{
    public class ClassicSudokuViewModel : BaseSudokuViewModel
    {
        #region Properties

        public bool AreDiagonalRulesApplied
        {
            get
            {
                if ((BaseClassicSudokuTableViewModel)SudokuBoardControl?.DataContext != null)
                {
                    return ((BaseClassicSudokuTableViewModel)SudokuBoardControl?.DataContext).AreDiagonalRulesApplied;
                }
                return false;
            }
            set
            {
                if ((BaseClassicSudokuTableViewModel)SudokuBoardControl?.DataContext != null)
                {
                    ((BaseClassicSudokuTableViewModel)SudokuBoardControl?.DataContext).AreDiagonalRulesApplied = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SudokuBoardSize> SudokuBoardSizes => SudokuBoard.SudokuBoardSizes;

        #endregion

        #region Constructor

        public ClassicSudokuViewModel() : base()
        {
            LoadCommands();
            Draw(SudokuBoard.SudokuBoardSizes[0]);
        }

        public ClassicSudokuViewModel(IMessageBoxService messageBoxService) : base(messageBoxService)
        {
            LoadCommands();
            Draw(SudokuBoard.SudokuBoardSizes[0]);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Draws the classic sudoku table with the given size.
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

            bool oldDiagonalRulesStatus = AreDiagonalRulesApplied;
            if (sudokuBoardSize.Height == 9 && sudokuBoardSize.Width == 9)
            {
                SudokuBoardControl = new UcClassicSudoku9x9Table();
                _actualSudokuBoard = CreateBoard(sudokuBoardSize, (BaseSudokuTableViewModel)SudokuBoardControl.DataContext, false);
            }
            else if (sudokuBoardSize.Height == 6 && sudokuBoardSize.Width == 6)
            {
                SudokuBoardControl = new UcClassicSudoku6x6Table();
                _actualSudokuBoard = CreateBoard(sudokuBoardSize, (BaseSudokuTableViewModel)SudokuBoardControl.DataContext, false);
            }
            else
            {
                SudokuBoardControl = new UcClassicSudoku4x4Table();
                _actualSudokuBoard = CreateBoard(sudokuBoardSize, (BaseSudokuTableViewModel)SudokuBoardControl.DataContext, false);
            }

            ((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).AreDiagonalRulesApplied = oldDiagonalRulesStatus;
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
                    Resources.MessageBox_Question_Title, MessageBoxButton.YesNo, MessageBoxImage.Question, (Style)Application.Current?.Resources["MessageBoxStyleForSudokuSolve"]);

                if (msgBoxResult != MessageBoxResult.Cancel)
                {
                    Solutions.Clear();
                    SolutionCounter = string.Empty;
                    _actualSudokuBoard = CreateBoard(_actualSudokuBoard.BoardSize, (BaseSudokuTableViewModel)SudokuBoardControl.DataContext, true, AreDiagonalRulesApplied);

                    if (msgBoxResult == MessageBoxResult.Yes)
                    {
                        BusyIndicatorContent = new UcSolvingBusyIndicatorContent();
                        _sudokuSolverThread = new Thread(CountAllSolutions);
                        _sudokuSolverThread.Start();
                    }
                    else
                    {
                        _sudokuSolverThread = new Thread(CountOneSolution);
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
                    BusyIndicatorContent = new UcSavingBusyIndicatorContent();
                    var classicSudokuFile = new ClassicSudokuFile();
                    classicSudokuFile.SelectedSudokuBoardSize = SelectedSudokuBoardSize;
                    classicSudokuFile.AreDiagonalRulesSet = ((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).AreDiagonalRulesApplied;
                    classicSudokuFile.Cells = new ObservableCollection<ObservableCollection<string>>(((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).Cells);
                    classicSudokuFile.BoldCells = new ObservableCollection<ObservableCollection<bool>>(((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).BoldCells);
                    classicSudokuFile.Solutions = _solutions;
                    classicSudokuFile.SolutionIndex = _solutionIndex;
                    classicSudokuFile.SolutionCounter = SolutionCounter;
                    classicSudokuFile.IsSolutionCounterVisible = IsSolutionCounterVisible;

                    _sudokuSavingThread = new Thread(() =>
                    {
                        try
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.ApplicationIdle,
                            new Action(() => SerializeClassicSudokuFile(_saveFileDialog.FileName, classicSudokuFile)));
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
                    BusyIndicatorContent = new UcLoadingBusyIndicatorContent();
                    _sudokuLoadingThread = new Thread(() =>
                    {
                        try
                        {
                            ClassicSudokuFile classicSudokuFile = null;
                            using (Stream stream = File.Open(_openFileDialog.FileName, FileMode.Open))
                            {
                                var bformatter = new BinaryFormatter();
                                classicSudokuFile = (ClassicSudokuFile)bformatter.Deserialize(stream);
                            }
                            Application.Current.Dispatcher.Invoke(new Action(() => LoadDeserializedClassicSudokuFile(classicSudokuFile)));
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
        /// Serializes the given classic sudoku file into the given file.
        /// </summary>
        /// <param name="filePath">File where the classic sudoku file will be serialized.</param>
        /// <param name="classicSudokuFile">Classic sudoku file.</param>
        void SerializeClassicSudokuFile(string filePath, ClassicSudokuFile classicSudokuFile)
        {
            using (Stream stream = File.Open(filePath, FileMode.Create))
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, classicSudokuFile);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Loads the deserialized classic sudoku file.
        /// </summary>
        /// <param name="classicSudokuFile">Classic sudoku file</param>
        void LoadDeserializedClassicSudokuFile(ClassicSudokuFile classicSudokuFile)
        {
            IsBusy = false;
            SelectedSudokuBoardSize = classicSudokuFile.SelectedSudokuBoardSize;
            Draw(SelectedSudokuBoardSize);
            AreDiagonalRulesApplied = classicSudokuFile.AreDiagonalRulesSet;
            ((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).Cells = classicSudokuFile.Cells;
            ((BaseClassicSudokuTableViewModel)SudokuBoardControl.DataContext).BoldCells = classicSudokuFile.BoldCells;
            SolutionCounter = string.Empty;
            _solutions = classicSudokuFile.Solutions;
            _solutionIndex = classicSudokuFile.SolutionIndex;
            SolutionCounter = classicSudokuFile.SolutionCounter;
            IsSolutionCounterVisible = classicSudokuFile.IsSolutionCounterVisible;

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

        #endregion
    }
}