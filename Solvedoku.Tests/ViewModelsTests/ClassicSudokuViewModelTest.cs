using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Solvedoku.ViewModels.ClassicSudoku;
using Solvedoku.Classes;
using Solvedoku.Commands;
using Solvedoku.Views.ClassicSudoku;
using Solvedoku.ViewModels;
using Moq;
using Solvedoku.Services.MessageBox;
using System.Windows;
using System.Threading.Tasks;
using System.Threading;

namespace Solvedoku.Tests.ViewModelsTests
{
    /// <summary>
    /// Summary description for ClassicSudokuViewModelTest
    /// </summary>
    [TestClass]
    public class ClassicSudokuViewModelTest
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            if (Application.Current == null)
            {
                new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            }
        }

        [TestMethod]
        public void Draw2x2TableTest()
        {
            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel();
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 4;
            sudokuBoardSize.Width = 4;
            sudokuBoardSize.BoxCountX = 2;
            sudokuBoardSize.BoxCountY = 2;

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);
            object expectedBoard = classicSudokuViewModel.SudokuBoardControl;
            Assert.AreEqual(typeof(UcClassicSudoku4x4Table), expectedBoard.GetType());
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void Draw6x6TableTest()
        {
            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel();
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 6;
            sudokuBoardSize.Width = 6;
            sudokuBoardSize.BoxCountX = 2;
            sudokuBoardSize.BoxCountY = 3;

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);
            object expectedBoard = classicSudokuViewModel.SudokuBoardControl;
            Assert.AreEqual(typeof(UcClassicSudoku6x6Table), expectedBoard.GetType());
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void Draw9x9TableTest()
        {
            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel();
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 9;
            sudokuBoardSize.Width = 9;
            sudokuBoardSize.BoxCountX = 3;
            sudokuBoardSize.BoxCountY = 3;

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);
            object expectedBoard = classicSudokuViewModel.SudokuBoardControl;
            Assert.AreEqual(typeof(UcClassicSudoku9x9Table), expectedBoard.GetType());
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void Draw2x2TableWithAlreadyFilledCellsTest()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(messageBoxMock.Object);
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 4;
            sudokuBoardSize.Width = 4;
            sudokuBoardSize.BoxCountX = 2;
            sudokuBoardSize.BoxCountY = 2;

            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "1";
            tableViewModel.Cells[0][1] = "2";

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();

            Assert.AreEqual(typeof(UcClassicSudoku4x4Table), actualBoard.GetType());
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][0]);
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][1]);
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void Draw6x6TableWithAlreadyFilledCellsTest()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(messageBoxMock.Object);
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 6;
            sudokuBoardSize.Width = 6;
            sudokuBoardSize.BoxCountX = 2;
            sudokuBoardSize.BoxCountY = 3;

            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "1";
            tableViewModel.Cells[0][1] = "2";

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();

            Assert.AreEqual(typeof(UcClassicSudoku6x6Table), actualBoard.GetType());
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][0]);
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][1]);
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void Draw9x9TableWithAlreadyFilledCellsTest()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(messageBoxMock.Object);
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 9;
            sudokuBoardSize.Width = 9;
            sudokuBoardSize.BoxCountX = 3;
            sudokuBoardSize.BoxCountY = 3;

            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "1";
            tableViewModel.Cells[0][1] = "2";

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();

            Assert.AreEqual(typeof(UcClassicSudoku9x9Table), actualBoard.GetType());
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][0]);
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][1]);
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }

        public void DisplaySolutionAndMessageTest()
        {
            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(new MessageBoxService());
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 4;
            sudokuBoardSize.Width = 4;
            sudokuBoardSize.BoxCountX = 2;
            sudokuBoardSize.BoxCountY = 2;
            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);
            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();

            SudokuBoard sudokuBoard = classicSudokuViewModel.CreateBoard(sudokuBoardSize, tableViewModel, true);
        }

        [TestMethod]
        public void GetOneSolutionFor4x4TableWithPredefinedCells()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(messageBoxMock.Object);
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 4;
            sudokuBoardSize.Width = 4;
            sudokuBoardSize.BoxCountX = 2;
            sudokuBoardSize.BoxCountY = 2;

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "1";
            tableViewModel.Cells[0][1] = "2";
            tableViewModel.Cells[0][2] = "3";
            tableViewModel.Cells[0][3] = "4";

            classicSudokuViewModel.ActualSudokuBoard = classicSudokuViewModel.CreateBoard(sudokuBoardSize, tableViewModel, true, classicSudokuViewModel.AreDiagonalRulesApplied);
            classicSudokuViewModel.CountOneSolution();

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            
            
            Assert.AreEqual(typeof(UcClassicSudoku4x4Table), actualBoard.GetType());
            Assert.AreEqual("1", tableViewModel.Cells[0][0]);
            Assert.AreEqual("2", tableViewModel.Cells[0][1]);
            Assert.AreEqual("3", tableViewModel.Cells[0][2]);
            Assert.AreEqual("4", tableViewModel.Cells[0][3]);
            Assert.AreEqual("3", tableViewModel.Cells[1][0]);
            Assert.AreEqual("4", tableViewModel.Cells[1][1]);
            Assert.AreEqual("1", tableViewModel.Cells[1][2]);
            Assert.AreEqual("2", tableViewModel.Cells[1][3]);
            Assert.AreEqual("2", tableViewModel.Cells[2][0]);
            Assert.AreEqual("1", tableViewModel.Cells[2][1]);
            Assert.AreEqual("4", tableViewModel.Cells[2][2]);
            Assert.AreEqual("3", tableViewModel.Cells[2][3]);
            Assert.AreEqual("4", tableViewModel.Cells[3][0]);
            Assert.AreEqual("3", tableViewModel.Cells[3][1]);
            Assert.AreEqual("2", tableViewModel.Cells[3][2]);
            Assert.AreEqual("1", tableViewModel.Cells[3][3]);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(1, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void GetOneSolutionFor6x6TableWithPredefinedCells()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(messageBoxMock.Object);
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 6;
            sudokuBoardSize.Width = 6;
            sudokuBoardSize.BoxCountX = 3;
            sudokuBoardSize.BoxCountY = 2;

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "1";
            tableViewModel.Cells[0][1] = "2";
            tableViewModel.Cells[0][2] = "3";
            tableViewModel.Cells[0][3] = "4";
            tableViewModel.Cells[0][4] = "5";
            tableViewModel.Cells[0][5] = "6";

            classicSudokuViewModel.ActualSudokuBoard = classicSudokuViewModel.CreateBoard(sudokuBoardSize, tableViewModel, true, classicSudokuViewModel.AreDiagonalRulesApplied);
            classicSudokuViewModel.CountOneSolution();

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();


            Assert.AreEqual(typeof(UcClassicSudoku6x6Table), actualBoard.GetType());
            Assert.AreEqual("1", tableViewModel.Cells[0][0]);
            Assert.AreEqual("2", tableViewModel.Cells[0][1]);
            Assert.AreEqual("3", tableViewModel.Cells[0][2]);
            Assert.AreEqual("4", tableViewModel.Cells[0][3]);
            Assert.AreEqual("5", tableViewModel.Cells[0][4]);
            Assert.AreEqual("6", tableViewModel.Cells[0][5]);

            Assert.AreEqual("3", tableViewModel.Cells[1][0]);
            Assert.AreEqual("4", tableViewModel.Cells[1][1]);
            Assert.AreEqual("5", tableViewModel.Cells[1][2]);
            Assert.AreEqual("6", tableViewModel.Cells[1][3]);
            Assert.AreEqual("1", tableViewModel.Cells[1][4]);
            Assert.AreEqual("2", tableViewModel.Cells[1][5]);

            Assert.AreEqual("5", tableViewModel.Cells[2][0]);
            Assert.AreEqual("6", tableViewModel.Cells[2][1]);
            Assert.AreEqual("1", tableViewModel.Cells[2][2]);
            Assert.AreEqual("2", tableViewModel.Cells[2][3]);
            Assert.AreEqual("3", tableViewModel.Cells[2][4]);
            Assert.AreEqual("4", tableViewModel.Cells[2][5]);

            Assert.AreEqual("2", tableViewModel.Cells[3][0]);
            Assert.AreEqual("1", tableViewModel.Cells[3][1]);
            Assert.AreEqual("4", tableViewModel.Cells[3][2]);
            Assert.AreEqual("3", tableViewModel.Cells[3][3]);
            Assert.AreEqual("6", tableViewModel.Cells[3][4]);
            Assert.AreEqual("5", tableViewModel.Cells[3][5]);

            Assert.AreEqual("4", tableViewModel.Cells[4][0]);
            Assert.AreEqual("3", tableViewModel.Cells[4][1]);
            Assert.AreEqual("6", tableViewModel.Cells[4][2]);
            Assert.AreEqual("5", tableViewModel.Cells[4][3]);
            Assert.AreEqual("2", tableViewModel.Cells[4][4]);
            Assert.AreEqual("1", tableViewModel.Cells[4][5]);

            Assert.AreEqual("6", tableViewModel.Cells[5][0]);
            Assert.AreEqual("5", tableViewModel.Cells[5][1]);
            Assert.AreEqual("2", tableViewModel.Cells[5][2]);
            Assert.AreEqual("1", tableViewModel.Cells[5][3]);
            Assert.AreEqual("4", tableViewModel.Cells[5][4]);
            Assert.AreEqual("3", tableViewModel.Cells[5][5]);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(1, classicSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void GetOneSolutionFor9x9TableWithPredefinedCells()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);

            ClassicSudokuViewModel classicSudokuViewModel = new ClassicSudokuViewModel(messageBoxMock.Object);
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 6;
            sudokuBoardSize.Width = 6;
            sudokuBoardSize.BoxCountX = 3;
            sudokuBoardSize.BoxCountY = 2;

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            BaseSudokuTableViewModel tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "1";
            tableViewModel.Cells[0][1] = "2";
            tableViewModel.Cells[0][2] = "3";
            tableViewModel.Cells[0][3] = "4";
            tableViewModel.Cells[0][4] = "5";
            tableViewModel.Cells[0][5] = "6";

            classicSudokuViewModel.ActualSudokuBoard = classicSudokuViewModel.CreateBoard(sudokuBoardSize, tableViewModel, true, classicSudokuViewModel.AreDiagonalRulesApplied);
            classicSudokuViewModel.CountOneSolution();

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();


            Assert.AreEqual(typeof(UcClassicSudoku6x6Table), actualBoard.GetType());
            Assert.AreEqual("1", tableViewModel.Cells[0][0]);
            Assert.AreEqual("2", tableViewModel.Cells[0][1]);
            Assert.AreEqual("3", tableViewModel.Cells[0][2]);
            Assert.AreEqual("4", tableViewModel.Cells[0][3]);
            Assert.AreEqual("5", tableViewModel.Cells[0][4]);
            Assert.AreEqual("6", tableViewModel.Cells[0][5]);

            Assert.AreEqual("3", tableViewModel.Cells[1][0]);
            Assert.AreEqual("4", tableViewModel.Cells[1][1]);
            Assert.AreEqual("5", tableViewModel.Cells[1][2]);
            Assert.AreEqual("6", tableViewModel.Cells[1][3]);
            Assert.AreEqual("1", tableViewModel.Cells[1][4]);
            Assert.AreEqual("2", tableViewModel.Cells[1][5]);

            Assert.AreEqual("5", tableViewModel.Cells[2][0]);
            Assert.AreEqual("6", tableViewModel.Cells[2][1]);
            Assert.AreEqual("1", tableViewModel.Cells[2][2]);
            Assert.AreEqual("2", tableViewModel.Cells[2][3]);
            Assert.AreEqual("3", tableViewModel.Cells[2][4]);
            Assert.AreEqual("4", tableViewModel.Cells[2][5]);

            Assert.AreEqual("2", tableViewModel.Cells[3][0]);
            Assert.AreEqual("1", tableViewModel.Cells[3][1]);
            Assert.AreEqual("4", tableViewModel.Cells[3][2]);
            Assert.AreEqual("3", tableViewModel.Cells[3][3]);
            Assert.AreEqual("6", tableViewModel.Cells[3][4]);
            Assert.AreEqual("5", tableViewModel.Cells[3][5]);

            Assert.AreEqual("4", tableViewModel.Cells[4][0]);
            Assert.AreEqual("3", tableViewModel.Cells[4][1]);
            Assert.AreEqual("6", tableViewModel.Cells[4][2]);
            Assert.AreEqual("5", tableViewModel.Cells[4][3]);
            Assert.AreEqual("2", tableViewModel.Cells[4][4]);
            Assert.AreEqual("1", tableViewModel.Cells[4][5]);

            Assert.AreEqual("6", tableViewModel.Cells[5][0]);
            Assert.AreEqual("5", tableViewModel.Cells[5][1]);
            Assert.AreEqual("2", tableViewModel.Cells[5][2]);
            Assert.AreEqual("1", tableViewModel.Cells[5][3]);
            Assert.AreEqual("4", tableViewModel.Cells[5][4]);
            Assert.AreEqual("3", tableViewModel.Cells[5][5]);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(1, classicSudokuViewModel.Solutions.Count);
        }
    }
}