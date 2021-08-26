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
        public void GetOneSolutionFor2x2TableWithPredefinedCells()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.No);

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

            var autoResetEvent = new AutoResetEvent(false);
            var timerFired = false;

            new Timer(x =>
            {
                timerFired = true;
                ((ParameterlessCommand)classicSudokuViewModel.SolveSudokuCommand).Execute();
                autoResetEvent.Set();
            }, null, 2000, Timeout.Infinite);

            autoResetEvent.WaitOne(2000);
            
            
            //Thread.Sleep(30000);
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
    }
}