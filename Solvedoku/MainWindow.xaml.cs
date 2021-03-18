using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Solvedoku.Classes;
using Xceed.Wpf.Toolkit;

namespace Solvedoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        SaveFileDialog SaveFile = new SaveFileDialog();
        OpenFileDialog LoadFile = new OpenFileDialog();
       
        private Thread _sudokuSolverThread;
        private int _classicSolutionIndex = 0;
        private int _actualClassicSizeIndex = 0;     
        private List<SudokuBoard> _classicSolutions = new List<SudokuBoard>();
        private SudokuClassicHandler _classicHandler = new SudokuClassicHandler();

        private Thread _puzzleSolverThread;
        private int _puzzleSolutionIndex = 0;
        private ObservableCollection<ColorItem> _puzzleColorList;
        public ObservableCollection<ColorItem> PuzzleColorList
        {
            get { return _puzzleColorList; }
            set { _puzzleColorList = value; }
        }
        private List<SudokuBoard> _puzzleSolutions = new List<SudokuBoard>();
        private SudokuPuzzleHandler _puzzleHandler = new SudokuPuzzleHandler();


        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            PopulatePuzzleColorList();
            DataContext = this;
        }

        #endregion

        //#region ClassicSudoku

        //#region Events
        //private void BtSolveClassic_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MessageBoxResult msgBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show(this,
        //  "Kérlek válaszd ki, hogy az összes megoldást (feltéve ha van egynél több, illetve ez időigényes is lehet), vagy csak egy lehetségeset szeretnél megkapni.",
        //  "Kérdés!",
        //  MessageBoxButton.YesNo, MessageBoxImage.Question,
        //  (Style)Application.Current.Resources["MessageBoxStyleForClassicSolve"]);

        //        if (msgBoxResult != MessageBoxResult.Cancel)
        //        {

        //            Grid actClassicGrid = (Grid)this.FindName("ClassicGrid");

        //            BtClassicRight.IsEnabled = false;
        //            BtClassicLeft.IsEnabled = false;
        //            var board = CreateClassicBoard(actClassicGrid.RowDefinitions.Count, actClassicGrid.ColumnDefinitions.Count, CountBlockSize(actClassicGrid.RowDefinitions.Count).Item1, CountBlockSize(actClassicGrid.RowDefinitions.Count).Item2);
        //            _classicSolutions.Clear();
        //            _puzzleSolutionIndex = 0;
        //            LbClassicSolvesCount.Content = "";

        //            if (msgBoxResult == MessageBoxResult.Yes)
        //            {

        //                _sudokuSolverThread = new Thread(() =>
        //                {
        //                    _classicSolutions = Sudoku_SolverThread(board, true);
        //                    Action action = DisplayClassicSolutionAndMessage;
        //                    Dispatcher.BeginInvoke(action);
        //                });
        //                _sudokuSolverThread.Start();

        //            }
        //            else
        //            {
        //                _sudokuSolverThread = new Thread(() =>
        //                {
        //                    _classicSolutions = Sudoku_SolverThread(board, false);
        //                    Action action = DisplayClassicSolutionAndMessage;
        //                    Dispatcher.BeginInvoke(action);
        //                });
        //                _sudokuSolverThread.Start();

        //            }
        //            BusyIClassic.IsBusy = true;
        //        }
        //    }
        //    catch
        //    {
        //        Xceed.Wpf.Toolkit.MessageBox.Show(this, "A klasszikus feladvány megoldása során hiba lépett fel. Kérlek ellenőrizd, hogy helyesen adtad-e meg a feladatot.",
        //            "Hiba!",
        //            MessageBoxButton.OK, MessageBoxImage.Error,
        //            (Style)Application.Current.Resources["MessageBoxStyle"]);
        //    }

        //}
        //private void BtSaveClassic_OnClick(object sender, RoutedEventArgs e)
        //{
        //    SaveFile.Title = "Klasszikus SUDOKU mentése..";
        //    SaveFile.RestoreDirectory = true;
        //    SaveFile.DefaultExt = "csu";
        //    SaveFile.Filter = "Klasszikus SUDOKU fájlok (*.csu)|*.csu";
        //    SaveFile.FilterIndex = 1;
        //    SaveFile.CheckPathExists = true;
        //    SaveFile.OverwritePrompt = true;

        //    if (SaveFile.ShowDialog().GetValueOrDefault())
        //    {
        //        try
        //        {
        //            SudokuClassicHandler scHandler = new SudokuClassicHandler();
        //            Grid actClassicGrid = (Grid)this.FindName("ClassicGrid");
        //            scHandler.ActClassicBoard = CreateClassicBoard(actClassicGrid.RowDefinitions.Count, actClassicGrid.ColumnDefinitions.Count, CountBlockSize(actClassicGrid.RowDefinitions.Count).Item1, CountBlockSize(actClassicGrid.RowDefinitions.Count).Item2);
        //            scHandler.ActClassicSolutions = _classicSolutions;
        //            scHandler.SelectedSizeIndex = _actualClassicSizeIndex;

        //            using (Stream stream = File.Open(SaveFile.FileName, FileMode.Create))
        //            {
        //                var bformatter = new BinaryFormatter();
        //                bformatter.Serialize(stream, scHandler);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Xceed.Wpf.Toolkit.MessageBox messageBox = new Xceed.Wpf.Toolkit.MessageBox();
        //            messageBox.Background = Brushes.Gray;
        //            messageBox.Caption = "Hiba!";
        //            messageBox.Text = "A SUDOKU mentése során hiba lépett fel.";
        //            messageBox.ButtonRegionBackground = Brushes.Gray;
        //            Xceed.Wpf.Toolkit.MessageBox.Show(this, messageBox.Text, messageBox.Caption, MessageBoxButton.OK,
        //                MessageBoxImage.Error, (Style)Application.Current.Resources["MessageBoxStyle"]);
        //        }
        //    }
        //    SaveFile.Reset();
        //}
        //private void BtDrawClassic_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (Xceed.Wpf.Toolkit.MessageBox.Show(this, "Új sudoku rajzolásánál minden szám törlődik. Biztos, hogy ezt szeretnéd?", "Figyelmeztetés!", MessageBoxButton.YesNo, MessageBoxImage.Warning, (Style)Application.Current.Resources["MessageBoxStyle"]) == MessageBoxResult.Yes)
        //        {
        //            LbClassicSolvesCount.Content = "";

        //            UnregisterClassicControls();

        //            ComboBoxItem currentSize = (ComboBoxItem)CboxClassicSizes.SelectedItem;
        //            string[] parameters = currentSize.Tag.ToString().Split('x');

        //            int actHeight = Convert.ToInt32(parameters[0]);
        //            int actWidth = Convert.ToInt32(parameters[1]);
        //            int actBlockHeight = Convert.ToInt32(parameters[2]);
        //            int actBlockWidth = Convert.ToInt32(parameters[3]);

        //            DrawClassic(actHeight, actWidth, actBlockHeight, actBlockWidth);
        //            _actualClassicSizeIndex = CboxClassicSizes.SelectedIndex;
        //        }
        //    }
        //    catch
        //    {
        //        Xceed.Wpf.Toolkit.MessageBox.Show(this, "A klasszikus SUDOKU rajzolása során hiba lépett fel. Kérlek lépj kapcsolatba a program készítőjével.",
        //            "Hiba!",
        //            MessageBoxButton.OK, MessageBoxImage.Error,
        //            (Style)Application.Current.Resources["MessageBoxStyle"]);
        //    }       
        //}
        //private void BtClassicRight_OnClick(object sender, RoutedEventArgs e)
        //{
        //    _classicSolutionIndex++;
        //    BtClassicLeft.IsEnabled = true;
        //    string[,] actSolution = _classicSolutions[_classicSolutionIndex].OutputAsMatrix();
        //    DisplayBoard(actSolution, "TbClassicCell");
        //    LbClassicSolvesCount.Content = "Megoldások: " + (_classicSolutionIndex + 1) + "/" + _classicSolutions.Count;
        //    if (_classicSolutionIndex == (_classicSolutions.Count - 1))
        //    {
        //        BtClassicRight.IsEnabled = false;
        //    }
        //}
        //private void BtClassicLeft_OnClick(object sender, RoutedEventArgs e)
        //{
        //    _classicSolutionIndex--;
        //    BtClassicRight.IsEnabled = true;
        //    string[,] actSolution = _classicSolutions[_classicSolutionIndex].OutputAsMatrix();
        //    DisplayBoard(actSolution, "TbClassicCell");
        //    LbClassicSolvesCount.Content = "Megoldások: " + (_classicSolutionIndex + 1) + "/" + _classicSolutions.Count;
        //    if (_classicSolutionIndex == 0)
        //    {
        //        BtClassicLeft.IsEnabled = false;
        //    }
        //}
        //private void BtLoadClassic_OnClick(object sender, RoutedEventArgs e)
        //{
        //    LoadFile.Title = "Klasszikus SUDOKU betöltése..";
        //    LoadFile.RestoreDirectory = true;
        //    LoadFile.DefaultExt = "csu";
        //    LoadFile.Filter = "Klasszikus SUDOKU fájlok (*.csu)|*.csu";
        //    LoadFile.FilterIndex = 1;
        //    LoadFile.CheckPathExists = true;
        //    LoadFile.CheckFileExists = true;


        //    if (LoadFile.ShowDialog().GetValueOrDefault())
        //    {
        //        try
        //        {
        //            using (Stream stream = File.Open(LoadFile.FileName, FileMode.Open))
        //            {
        //                var bformatter = new BinaryFormatter();
        //                _classicHandler = (SudokuClassicHandler)bformatter.Deserialize(stream);

        //            }

        //            Tuple<int, int> blockSize = CountBlockSize(_classicHandler.ActClassicBoard.Height);
        //            CboxClassicSizes.SelectedIndex = _classicHandler.SelectedSizeIndex;
        //            _actualClassicSizeIndex = _classicHandler.SelectedSizeIndex;

        //            UnregisterClassicControls();
        //            LbClassicSolvesCount.Content = "";
        //            DrawClassic(_classicHandler.ActClassicBoard.Height, _classicHandler.ActClassicBoard.Width, blockSize.Item1, blockSize.Item2);
        //            DisplayBoard(_classicHandler.ActClassicBoard.OutputAsMatrix(), "TbClassicCell");

        //            _classicSolutions = _classicHandler.ActClassicSolutions;
        //            if (_classicSolutions.Count > 1)
        //            {
        //                Xceed.Wpf.Toolkit.MessageBox.Show(this, "A betöltött klasszikus feladványnak több megoldása is van (összesen " + _classicSolutions.Count + "). A táblázat alatt található nyilakkal tudsz köztük váltani.", "Információ!",
        //                    MessageBoxButton.OK, MessageBoxImage.Information,
        //                    (Style)Application.Current.Resources["MessageBoxStyle"]);

        //                LbClassicSolvesCount.Content = "Megoldások: 1/" + _classicSolutions.Count;
        //                LbClassicSolvesCount.Visibility = Visibility.Visible;
        //                BtClassicRight.IsEnabled = true;
        //                BtClassicLeft.IsEnabled = false;
        //            }
        //            else if (_classicSolutions.Count == 1)
        //            {
        //                Xceed.Wpf.Toolkit.MessageBox.Show(this, "A betöltött klasszikus feladványnak egy megoldása van.", "Információ!",
        //                    MessageBoxButton.OK, MessageBoxImage.Information,
        //                    (Style)Application.Current.Resources["MessageBoxStyle"]);
        //                LbClassicSolvesCount.Content = "";
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            Xceed.Wpf.Toolkit.MessageBox messageBox = new Xceed.Wpf.Toolkit.MessageBox();
        //            messageBox.Background = Brushes.Gray;
        //            messageBox.Caption = "Hiba!";
        //            messageBox.Text = "A klasszikus SUDOKU betöltése során hiba lépett fel. " + ex.Message;
        //            messageBox.ButtonRegionBackground = Brushes.Gray;
        //            Xceed.Wpf.Toolkit.MessageBox.Show(this, messageBox.Text, messageBox.Caption, MessageBoxButton.OK,
        //                MessageBoxImage.Error, (Style)Application.Current.Resources["MessageBoxStyle"]);
        //        }
        //    }
        //    LoadFile.Reset();
        //}
        //private void TbClassicCell_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    Grid actClassicGrid = (Grid)this.FindName("ClassicGrid");
        //    string maxNumber = actClassicGrid.RowDefinitions.Count.ToString();

        //    if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-" + maxNumber + "]") || ((TextBox)sender).Text.Length >= 2)
        //    {
        //        Xceed.Wpf.Toolkit.MessageBox.Show(this, "Csak egyjegyű számokat üthetsz be 1-től " + maxNumber + "-ig.", "Információ!",
        //            MessageBoxButton.OK, MessageBoxImage.Information,
        //            (Style)Application.Current.Resources["MessageBoxStyle"]);
        //        ((TextBox)sender).Clear();
        //    }
        //}

        //#endregion

        //#region Methods
        //public void CloseClassicBusyIndicator()
        //{
        //    if (Xceed.Wpf.Toolkit.MessageBox.Show(this,
        //            "Biztos, hogy meg szeretnéd szakítani a megoldást?", "Figyelmeztetés!",
        //            MessageBoxButton.YesNo, MessageBoxImage.Warning,
        //            (Style)Application.Current.Resources["MessageBoxStyle"]) == MessageBoxResult.Yes)
        //    {
        //        _sudokuSolverThread.Abort();
        //        BusyIClassic.IsBusy = false;
        //    }
        //}
        //private void DisplayClassicSolutionAndMessage()
        //{
        //    BusyIClassic.IsBusy = false;
        //    if (_sudokuSolverThread.ThreadState != ThreadState.Aborted)
        //    {
        //        if (_classicSolutions.Count > 0 && _classicSolutions[0] != null)
        //        {
        //            if (_classicSolutions.Count > 1)
        //            {
        //                Xceed.Wpf.Toolkit.MessageBox.Show(this, "A klasszikus feladványnak több megoldása is van (összesen " + _classicSolutions.Count + "). A táblázat alatt található nyilakkal tudsz köztük váltani.", "Információ!",
        //                    MessageBoxButton.OK, MessageBoxImage.Information,
        //                    (Style)Application.Current.Resources["MessageBoxStyle"]);
        //                BtClassicRight.IsEnabled = true;
        //                LbClassicSolvesCount.Content = "Megoldások: 1/" + _classicSolutions.Count;
        //                LbClassicSolvesCount.Visibility = Visibility.Visible;
        //            }
        //            else
        //            {
        //                Xceed.Wpf.Toolkit.MessageBox.Show(this, "A klasszikus feladványnak egy megoldása van.", "Információ!",
        //                    MessageBoxButton.OK, MessageBoxImage.Information,
        //                    (Style)Application.Current.Resources["MessageBoxStyle"]);
        //            }

        //            string[,] actSolution = _classicSolutions[0].OutputAsMatrix();
        //            DisplayBoard(actSolution, "TbClassicCell");

        //        }
        //        else
        //        {
        //            Xceed.Wpf.Toolkit.MessageBox.Show(this, "A klasszikus feladványnak sajnos nincs megoldása.", "Információ!",
        //                MessageBoxButton.OK, MessageBoxImage.Information,
        //                (Style)Application.Current.Resources["MessageBoxStyle"]);
        //        }
        //    }
        //}
        //private SudokuBoard CreateClassicBoard(int height, int width, int blockHeight, int blockWidth)
        //{
        //    SudokuBoard board;

        //    if (blockHeight == 3 && blockWidth == 3)
        //    {
        //        board = SudokuFactory.ClassicWith3x3Boxes();

        //        for (int row = 0; row < 9; row++)
        //        {
        //            string actRow = "";
        //            for (int column = 0; column < 9; column++)
        //            {
        //                TextBox actCell = (TextBox)this.FindName("TbClassicCell" + column + row);
        //                if (actCell.Text == String.Empty)
        //                {
        //                    actRow += ".";
        //                }
        //                else
        //                {
        //                    // actCell.FontWeight = FontWeights.Bold;
        //                    actRow += actCell.Text;
        //                }
        //            }
        //            board.AddRow(actRow);
        //        }
        //    }
        //    else
        //    {
        //        board = SudokuFactory.SizeAndBoxes(width, height, blockHeight, blockWidth);
        //        for (int row = 0; row < height; row++)
        //        {
        //            string actRow = "";
        //            for (int column = 0; column < width; column++)
        //            {
        //                TextBox actCell = (TextBox)this.FindName("TbClassicCell" + column + row);
        //                if (actCell.Text == String.Empty)
        //                {
        //                    actRow += "0";
        //                }
        //                else
        //                {
        //                    //actCell.FontWeight = FontWeights.Bold;
        //                    actRow += actCell.Text;
        //                }
        //            }
        //            board.AddRow(actRow);
        //        }
        //    }


        //    return board;
        //}
        //private void UnregisterClassicControls()
        //{
        //    Grid classicGridToDelete = (Grid)LogicalTreeHelper.FindLogicalNode(ClassicDockPanel, "ClassicGrid");
        //    for (int row = 0; row < classicGridToDelete.RowDefinitions.Count; row++)
        //    {
        //        for (int column = 0; column < classicGridToDelete.ColumnDefinitions.Count; column++)
        //        {
        //            TextBox oldCell =
        //                (TextBox)LogicalTreeHelper.FindLogicalNode(classicGridToDelete, "TbClassicCell" + column + row);
        //            classicGridToDelete.UnregisterName(oldCell.Name);
        //            classicGridToDelete.Children.Remove(oldCell);
        //        }
        //    }

        //    ClassicDockPanel.Children.Remove(classicGridToDelete);
        //    ClassicDockPanel.UnregisterName(ClassicGrid.Name);

        //}
        //private void DrawClassic(int height, int width, int blockHeight, int blockWidth) //9x9(3x3) 4x4(2x2) 6x6(2x3)
        //{

        //    _classicSolutions.Clear();
        //    _classicSolutionIndex = 0;
        //    bool areEvenBlocks = blockHeight % 2 != 0;
        //    BtClassicRight.IsEnabled = false;
        //    BtClassicLeft.IsEnabled = false;

        //    Grid classicGrid = new Grid();
        //    classicGrid.Name = "ClassicGrid";
        //    classicGrid.SetValue(NameProperty, "ClassicGrid");
        //    classicGrid.Margin = new Thickness(8);
        //    classicGrid.HorizontalAlignment = HorizontalAlignment.Center;
        //    classicGrid.VerticalAlignment = VerticalAlignment.Center;
        //    classicGrid.Width = 400;
        //    classicGrid.Height = 400;
        //    ClassicDockPanel.Children.Add(classicGrid);
        //    ClassicDockPanel.RegisterName(classicGrid.Name, classicGrid);

        //    Style evenCellStyle = Application.Current.FindResource("TbSudokuEvenCellStyle") as Style;
        //    Style oddCellStyle = Application.Current.FindResource("TbSudokuCellStyle") as Style;

        //    Style actCellStyle = oddCellStyle;

        //    for (int i = 0; i < width; i++)
        //    {
        //        ColumnDefinition classicColumn = new ColumnDefinition();
        //        RowDefinition classicRow = new RowDefinition();
        //        classicGrid.ColumnDefinitions.Add(classicColumn);
        //        classicGrid.RowDefinitions.Add(classicRow);
        //    }

        //    for (int row = 0; row < height; row++)
        //    {
        //        if (row % blockHeight == 0 && row > 0)
        //        {
        //            if (actCellStyle == oddCellStyle)
        //            {
        //                actCellStyle = evenCellStyle;
        //            }
        //            else
        //            {
        //                actCellStyle = oddCellStyle;
        //            }
        //        }

        //        for (int column = 0; column < width; column++)
        //        {

        //            TextBox cell = new TextBox();
        //            cell.Style = actCellStyle;
        //            cell.TextChanged += TbClassicCell_OnTextChanged;

        //            cell.SetValue(NameProperty, "TbClassicCell" + column + row);
        //            cell.Name = "TbClassicCell" + column + row;
        //            classicGrid.RegisterName(cell.Name, cell);
        //            classicGrid.Children.Add(cell);


        //            Grid.SetRow(cell, row);
        //            Grid.SetColumn(cell, column);
        //            int nextColumn = column + 1;


        //            if (areEvenBlocks)
        //            {
        //                if (nextColumn % blockWidth == 0 && nextColumn != width)
        //                {
        //                    if (actCellStyle == oddCellStyle)
        //                    {
        //                        actCellStyle = evenCellStyle;
        //                    }
        //                    else
        //                    {
        //                        actCellStyle = oddCellStyle;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (nextColumn % blockWidth == 0)
        //                {
        //                    if (actCellStyle == oddCellStyle)
        //                    {
        //                        actCellStyle = evenCellStyle;
        //                    }
        //                    else
        //                    {
        //                        actCellStyle = oddCellStyle;
        //                    }
        //                }
        //            }

        //        }
        //    }





        //}

        //#endregion

        //#endregion

        #region PuzzleSudoku

        #region Events

        private void BtSolvePuzzle_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult msgBoxResult = Xceed.Wpf.Toolkit.MessageBox.Show(this,
                    "Kérlek válaszd ki, hogy az összes megoldást (feltéve ha van egynél több, illetve ez időigényes is lehet), vagy csak egy lehetségeset szeretnél megkapni.",
                    "Kérdés!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question,
                    (Style)Application.Current.Resources["MessageBoxStyleForClassicSolve"]);

                if (msgBoxResult != MessageBoxResult.Cancel)
                {
                    BtPuzzleRight.IsEnabled = false;
                    BtPuzzleLeft.IsEnabled = false;
                    LbPuzzleSolvesCount.Content = "";

                    string[] areas = GetPuzzleAreas();
                    var board = CreatePuzzleBoard(areas);

                    _puzzleSolutionIndex = 0;

                    if (msgBoxResult == MessageBoxResult.Yes)
                    {
                        _puzzleSolverThread = new Thread(() =>
                        {
                            _puzzleSolutions = Sudoku_SolverThread(board, true);
                            Action action = DisplayPuzzleSolutionAndMessage;
                            Dispatcher.BeginInvoke(action);
                        });
                        _puzzleSolverThread.Start();
                    }
                    else
                    {
                        _puzzleSolverThread = new Thread(() =>
                        {
                            _puzzleSolutions = Sudoku_SolverThread(board, false);
                            Action action = DisplayPuzzleSolutionAndMessage;
                            Dispatcher.BeginInvoke(action);
                        });
                        _puzzleSolverThread.Start();
                    }
                    BusyIPuzzle.IsBusy = true;
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(this, "A puzzle feladvány megoldása során hiba lépett fel. Kérlek ellenőrizd, hogy helyesen adtad-e meg a feladatot.",
                    "Hiba!",
                    MessageBoxButton.OK, MessageBoxImage.Error,
                    (Style)Application.Current.Resources["MessageBoxStyle"]);

            }

        }
        private void BtDrawPuzzle_OnClickPuzzle_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show(this, "Új sudoku rajzolásánál minden szám törlődik. Biztos, hogy ezt szeretnéd?", "Figyelmeztetés!", MessageBoxButton.YesNo, MessageBoxImage.Warning, (Style)Application.Current.Resources["MessageBoxStyle"]) == MessageBoxResult.Yes)
                {
                    LbPuzzleSolvesCount.Content = "";
                    DrawPuzzle(9, 9);
                }
            }
            catch
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(this, "A puzzle SUDOKU rajzolása során hiba lépett fel. Kérlek lépj kapcsolatba a program készítőjével.",
                    "Hiba!",
                    MessageBoxButton.OK, MessageBoxImage.Error,
                    (Style)Application.Current.Resources["MessageBoxStyle"]);
            }        
        }
        private void BtSavePuzzle_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFile.Title = "Puzzle SUDOKU mentése..";
            SaveFile.RestoreDirectory = true;
            SaveFile.DefaultExt = "psu";
            SaveFile.Filter = "Puzzle SUDOKU fájlok (*.psu)|*.psu";
            SaveFile.FilterIndex = 1;
            SaveFile.CheckPathExists = true;
            SaveFile.OverwritePrompt = true;

            if (SaveFile.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    SudokuPuzzleHandler spHandler = new SudokuPuzzleHandler();

                    spHandler.ActPuzzleBoard = CreatePuzzleBoard(GetPuzzleAreas());
                    spHandler.ActAreas = GetPuzzleAreas();
                    spHandler.ActPuzzleSolutions = _puzzleSolutions;

                    using (Stream stream = File.Open(SaveFile.FileName, FileMode.Create))
                    {
                        var bformatter = new BinaryFormatter();
                        bformatter.Serialize(stream, spHandler);
                    }
                }
                catch (Exception ex)
                {
                    Xceed.Wpf.Toolkit.MessageBox messageBox = new Xceed.Wpf.Toolkit.MessageBox();
                    messageBox.Background = Brushes.Gray;
                    messageBox.Caption = "Hiba!";
                    messageBox.Text = "A SUDOKU mentése során hiba lépett fel.";
                    messageBox.ButtonRegionBackground = Brushes.Gray;
                    Xceed.Wpf.Toolkit.MessageBox.Show(this, messageBox.Text, messageBox.Caption, MessageBoxButton.OK,
                        MessageBoxImage.Error, (Style)Application.Current.Resources["MessageBoxStyle"]);
                }
            }
            SaveFile.Reset();
        }
        private void BtLoadPuzzle_OnClick(object sender, RoutedEventArgs e)
        {
            LoadFile.Title = "Puzzle SUDOKU betöltése..";
            LoadFile.RestoreDirectory = true;
            LoadFile.DefaultExt = "psu";
            LoadFile.Filter = "Puzzle SUDOKU fájlok (*.psu)|*.psu";
            LoadFile.FilterIndex = 1;
            LoadFile.CheckPathExists = true;

            if (LoadFile.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    using (Stream stream = File.Open(LoadFile.FileName, FileMode.Open))
                    {
                        var bformatter = new BinaryFormatter();
                        _puzzleHandler = (SudokuPuzzleHandler)bformatter.Deserialize(stream);

                    }
                    UnregisterPuzzleControls();
                    LbPuzzleSolvesCount.Content = "";
                    DrawPuzzle(_puzzleHandler.ActPuzzleBoard.Height, _puzzleHandler.ActPuzzleBoard.Width);
                    DrawPuzzleAreas(_puzzleHandler.ActAreas);
                    DisplayBoard(_puzzleHandler.ActPuzzleBoard.OutputAsMatrix(), "TbPuzzleCell");
                    _puzzleSolutions = _puzzleHandler.ActPuzzleSolutions;
                    if (_puzzleSolutions.Count > 1)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(this, "A betöltött puzzle feladványnak több megoldása is van (összesen " + _puzzleSolutions.Count + "). A táblázat alatt található nyilakkal tudsz köztük váltani.", "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information,
                            (Style)Application.Current.Resources["MessageBoxStyle"]);

                        LbPuzzleSolvesCount.Content = "Megoldások: 1/" + _puzzleSolutions.Count;
                        LbPuzzleSolvesCount.Visibility = Visibility.Visible;
                        BtPuzzleRight.IsEnabled = true;
                        BtPuzzleLeft.IsEnabled = false;
                    }
                    else if (_puzzleSolutions.Count == 1)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(this, "A betöltött puzzle feladványnak egy megoldása van.", "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information,
                            (Style)Application.Current.Resources["MessageBoxStyle"]);

                        LbPuzzleSolvesCount.Content = "";
                    }
                }
                catch (Exception ex)
                {
                    Xceed.Wpf.Toolkit.MessageBox messageBox = new Xceed.Wpf.Toolkit.MessageBox();
                    messageBox.Background = Brushes.Gray;
                    messageBox.Caption = "Hiba!";
                    messageBox.Text = "A SUDOKU betöltése során hiba lépett fel. " + ex.Message;
                    messageBox.ButtonRegionBackground = Brushes.Gray;
                    Xceed.Wpf.Toolkit.MessageBox.Show(this, messageBox.Text, messageBox.Caption, MessageBoxButton.OK,
                        MessageBoxImage.Error, (Style)Application.Current.Resources["MessageBoxStyle"]);
                }
                LoadFile.Reset();
            }
        }
        private void BtPuzzleLeft_OnClick(object sender, RoutedEventArgs e)
        {
            _puzzleSolutionIndex--;
            BtPuzzleRight.IsEnabled = true;
            string[,] actSolution = _puzzleSolutions[_puzzleSolutionIndex].OutputAsMatrix();
            DisplayBoard(actSolution, "TbPuzzleCell");
            LbPuzzleSolvesCount.Content = "Megoldások: " + (_puzzleSolutionIndex + 1) + "/" + _puzzleSolutions.Count;
            if (_puzzleSolutionIndex == 0)
            {
                BtPuzzleLeft.IsEnabled = false;
            }
        }
        private void BtPuzzleRight_OnClick(object sender, RoutedEventArgs e)
        {
            _puzzleSolutionIndex++;
            BtPuzzleLeft.IsEnabled = true;
            string[,] actSolution = _puzzleSolutions[_puzzleSolutionIndex].OutputAsMatrix();
            DisplayBoard(actSolution, "TbPuzzleCell");
            LbPuzzleSolvesCount.Content = "Megoldások: " + (_puzzleSolutionIndex + 1) + "/" + _puzzleSolutions.Count;
            if (_puzzleSolutionIndex == (_puzzleSolutions.Count - 1))
            {
                BtPuzzleRight.IsEnabled = false;
            }
        }
        private void PuzzleCell_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            SolidColorBrush scBrush = new SolidColorBrush(PuzzleColorPicker.SelectedColor.GetValueOrDefault());
            ((TextBox)sender).Background = scBrush;
        }
        private void PuzzleCell_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                SolidColorBrush scBrush = new SolidColorBrush(PuzzleColorPicker.SelectedColor.GetValueOrDefault());
                ((TextBox)sender).Background = scBrush;
            }

        }
        private void TbPuzzleCell_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Grid actPuzzleGrid = (Grid)this.FindName("PuzzleGrid");
            string maxNumber = actPuzzleGrid.RowDefinitions.Count.ToString();

            if (System.Text.RegularExpressions.Regex.IsMatch(((TextBox)sender).Text, "[^1-" + maxNumber + "]") || ((TextBox)sender).Text.Length >= 2)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(this, "Csak egyjegyű számokat üthetsz be 1-től " + maxNumber + "-ig.", "Információ!",
                    MessageBoxButton.OK, MessageBoxImage.Information,
                    (Style)Application.Current.Resources["MessageBoxStyle"]);
                ((TextBox)sender).Clear();
            }
        }
        #endregion

        #region Methods


        private void PopulatePuzzleColorList()
        {
            _puzzleColorList = new ObservableCollection<ColorItem>();
            _puzzleColorList.Add(new ColorItem(Colors.LightBlue, "Világoskék"));
            _puzzleColorList.Add(new ColorItem(Colors.CornflowerBlue, "Égkék"));
            _puzzleColorList.Add(new ColorItem(Colors.Magenta, "Rózsaszín"));
            _puzzleColorList.Add(new ColorItem(Colors.Red, "Piros"));
            _puzzleColorList.Add(new ColorItem(Colors.Green, "Zöld"));
            _puzzleColorList.Add(new ColorItem(Colors.Yellow, "Sárga"));
            _puzzleColorList.Add(new ColorItem(Colors.RosyBrown, "Barna"));
            _puzzleColorList.Add(new ColorItem(Colors.Orange, "Narancssárga"));
            _puzzleColorList.Add(new ColorItem(Colors.MediumPurple, "Lila"));
            _puzzleColorList.Add(new ColorItem(Colors.LightGray, "Szürke"));
        }
        public void ClosePuzzleBusyIndicator()
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show(this,
                    "Biztos, hogy meg szeretnéd szakítani a megoldást?", "Figyelmeztetés!",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning,
                    (Style)Application.Current.Resources["MessageBoxStyle"]) == MessageBoxResult.Yes)
            {
                _puzzleSolverThread.Abort();
                BusyIPuzzle.IsBusy = false;
            }
        }
        private void DisplayPuzzleSolutionAndMessage()
        {
            BusyIPuzzle.IsBusy = false;
            if (_puzzleSolverThread.ThreadState != ThreadState.Aborted)
            {
                if (_puzzleSolutions.Count > 0 && _puzzleSolutions[0] != null)
                {
                    if (_puzzleSolutions.Count > 1)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(this,
                            "A puzzle feladványnak több megoldása is van (összesen " + _puzzleSolutions.Count +
                            "). A táblázat alatt található nyilakkal tudsz köztük váltani.", "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information,
                            (Style)Application.Current.Resources["MessageBoxStyle"]);
                        BtPuzzleRight.IsEnabled = true;
                        LbPuzzleSolvesCount.Content = "Megoldások: 1/" + _puzzleSolutions.Count;
                        LbPuzzleSolvesCount.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show(this, "A puzzle feladványnak egy megoldása van.",
                            "Információ!",
                            MessageBoxButton.OK, MessageBoxImage.Information,
                            (Style)Application.Current.Resources["MessageBoxStyle"]);
                    }

                    string[,] actSolution = _puzzleSolutions[0].OutputAsMatrix();
                    DisplayBoard(actSolution, "TbPuzzleCell");

                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(this, "A puzzle feladványnak sajnos nincs megoldása.",
                        "Információ!",
                        MessageBoxButton.OK, MessageBoxImage.Information,
                        (Style)Application.Current.Resources["MessageBoxStyle"]);
                }
            }
        }
        private string[] GetPuzzleAreas()
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
        }
        private int[,] ConvertPuzzleAreasToMatrix(string[] areas)
        {
            int[,] areaMatrix = new int[9, 9];
            for (int row = 0; row < areas.Length; row++)
            {
                int column = 0;
                foreach (Char number in areas[row])
                {
                    areaMatrix[row, column] = Convert.ToInt32(number.ToString());
                    column++;
                }
            }
            return areaMatrix;
        }
        private void DrawPuzzleAreas(string[] areas)
        {
            Grid puzzleGrid = (Grid)LogicalTreeHelper.FindLogicalNode(PuzzleDockPanel, "PuzzleGrid");
            List<SolidColorBrush> puzzleBrushes = new List<SolidColorBrush>();
            int[,] areaMatrix = ConvertPuzzleAreasToMatrix(areas);

            foreach (ColorItem cItem in PuzzleColorPicker.StandardColors)
            {
                puzzleBrushes.Add(new SolidColorBrush(cItem.Color.GetValueOrDefault()));
            }

            for (int row = 0; row < puzzleGrid.RowDefinitions.Count; row++)
            {
                for (int column = 0; column < puzzleGrid.ColumnDefinitions.Count; column++)
                {

                    TextBox actCell = (TextBox)this.FindName("TbPuzzleCell" + column + row);
                    actCell.Background = puzzleBrushes[areaMatrix[row, column]];

                }
            }
        }
        private void UnregisterPuzzleControls()
        {
            Grid puzzleGridToDelete = (Grid)LogicalTreeHelper.FindLogicalNode(PuzzleDockPanel, "PuzzleGrid");
            for (int row = 0; row < puzzleGridToDelete.RowDefinitions.Count; row++)
            {
                for (int column = 0; column < puzzleGridToDelete.ColumnDefinitions.Count; column++)
                {
                    TextBox oldCell =
                        (TextBox)LogicalTreeHelper.FindLogicalNode(puzzleGridToDelete, "TbPuzzleCell" + column + row);
                    puzzleGridToDelete.UnregisterName(oldCell.Name);
                    puzzleGridToDelete.Children.Remove(oldCell);
                }
            }

            PuzzleDockPanel.Children.Remove(puzzleGridToDelete);
            PuzzleDockPanel.UnregisterName(PuzzleGrid.Name);

        }
        private void DrawPuzzle(int height, int width) //9x9(3x3) 4x4(2x2) 6x6(2x3)
        {
            Grid puzzleGridToDelete = (Grid)LogicalTreeHelper.FindLogicalNode(PuzzleDockPanel, "PuzzleGrid");
            if (puzzleGridToDelete != null)
            {
                UnregisterPuzzleControls();
            }

            _puzzleSolutions.Clear();
            _puzzleSolutionIndex = 0;

            BtPuzzleRight.IsEnabled = false;
            BtPuzzleLeft.IsEnabled = false;

            Grid puzzleGrid = new Grid();
            puzzleGrid.Name = "PuzzleGrid";
            puzzleGrid.SetValue(NameProperty, "PuzzleGrid");
            puzzleGrid.Margin = new Thickness(8);
            puzzleGrid.HorizontalAlignment = HorizontalAlignment.Center;
            puzzleGrid.VerticalAlignment = VerticalAlignment.Center;
            puzzleGrid.Width = 400;
            puzzleGrid.Height = 400;
            PuzzleDockPanel.Children.Add(puzzleGrid);
            PuzzleDockPanel.RegisterName(puzzleGrid.Name, puzzleGrid);

            Style evenCellStyle = Application.Current.FindResource("TbSudokuEvenCellStyle") as Style;

            for (int i = 0; i < width; i++)
            {
                ColumnDefinition puzzleColumn = new ColumnDefinition();
                RowDefinition puzzleRow = new RowDefinition();
                puzzleGrid.ColumnDefinitions.Add(puzzleColumn);
                puzzleGrid.RowDefinitions.Add(puzzleRow);
            }

            for (int row = 0; row < height; row++)
            {

                for (int column = 0; column < width; column++)
                {

                    TextBox cell = new TextBox();
                    cell.Style = evenCellStyle;
                    cell.TextChanged += TbPuzzleCell_OnTextChanged;
                    cell.MouseRightButtonUp += PuzzleCell_OnMouseRightButtonUp;
                    cell.MouseMove += PuzzleCell_OnMouseMove;
                    cell.ContextMenu = null;

                    cell.SetValue(NameProperty, "TbPuzzleCell" + column + row);
                    cell.Name = "TbPuzzleCell" + column + row;
                    puzzleGrid.RegisterName(cell.Name, cell);
                    puzzleGrid.Children.Add(cell);


                    Grid.SetRow(cell, row);
                    Grid.SetColumn(cell, column);


                }
            }
        }
        private SudokuBoard CreatePuzzleBoard(string[] areas)
        {
            SudokuBoard board;
            board = SudokuFactory.ClassicWithSpecialBoxes(areas);

            for (int row = 0; row < 9; row++)
            {
                string actRow = "";
                for (int column = 0; column < 9; column++)
                {
                    TextBox actCell = (TextBox)this.FindName("TbPuzzleCell" + column + row);
                    if (actCell.Text == String.Empty)
                    {
                        actRow += ".";
                    }
                    else
                    {
                        //actCell.FontWeight = FontWeights.Bold;
                        actRow += actCell.Text;
                    }
                }
                board.AddRow(actRow);
            }
            return board;
        }

        #endregion

        #endregion

        #region Events
        //private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        LbClassicSolvesCount.Content = "";
        //        LbPuzzleSolvesCount.Content = "";
        //        ComboBoxItem currentSize = (ComboBoxItem)CboxClassicSizes.SelectedItem;
        //        string[] parameters = currentSize.Tag.ToString().Split('x');

        //        int actHeight = Convert.ToInt32(parameters[0]);
        //        int actWidth = Convert.ToInt32(parameters[1]);
        //        int actBlockHeight = Convert.ToInt32(parameters[2]);
        //        int actBlockWidth = Convert.ToInt32(parameters[3]);

        //        UnregisterClassicControls( /*actHeight, actWidth*/);
        //        DrawClassic(actHeight, actWidth, actBlockHeight, actBlockWidth);
        //        DrawPuzzle(9, 9);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.MessageBox.Show(ex.Message);
        //    }
        //}
        private List<SudokuBoard> Sudoku_SolverThread(object classicBoard, bool findAllSolutions)
        {
            List<SudokuBoard> solutions = new List<SudokuBoard>();

            if (findAllSolutions)
            {
                solutions = ((SudokuBoard)classicBoard).Solve().ToList();
            }
            else
            {
                solutions.Add(((SudokuBoard)classicBoard).SolveOnce());
            }

            return solutions;
        }
        #endregion

        #region Methods

        private void DisplayBoard(string[,] board, string cellTBName)
        {
            for (int row = 0; row < board.GetLength(1); row++)
            {
                for (int column = 0; column < board.GetLength(0); column++)
                {
                    TextBox actCell = (TextBox)this.FindName(cellTBName + column + row);
                    if (board[column, row] != "0")
                    {
                        actCell.Text = board[column, row];
                    }
                    UpdateLayout();

                }
            }
        }
        private Tuple<int, int> CountBlockSize(int size)
        {
            Tuple<int, int> blockSize = new Tuple<int, int>(0, 0);
            double sqrtSize = Math.Sqrt(size);

            if (sqrtSize % 1 == 0)
            {
                blockSize = new Tuple<int, int>((int)sqrtSize, (int)sqrtSize);
            }
            else
            {
                blockSize = new Tuple<int, int>((int)sqrtSize, (int)sqrtSize + 1);
            }
            return blockSize;
        }
        #endregion
    }
}