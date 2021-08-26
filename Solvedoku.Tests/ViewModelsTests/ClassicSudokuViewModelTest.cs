using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Solvedoku.ViewModels.ClassicSudoku;
using Solvedoku.Classes;
using Solvedoku.Views.ClassicSudoku;
using Solvedoku.ViewModels;
using Moq;
using Solvedoku.Services.MessageBox;
using System.Windows;

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

            classicSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);

            object actualBoard = classicSudokuViewModel.SudokuBoardControl;
            tableViewModel = classicSudokuViewModel.GetCurrentTableViewModel();

            Assert.AreEqual(typeof(UcClassicSudoku9x9Table), actualBoard.GetType());
            Assert.AreEqual(string.Empty, tableViewModel.Cells[0][0]);
            Assert.AreEqual(false, classicSudokuViewModel.AreDiagonalRulesApplied);
            Assert.AreEqual(string.Empty, classicSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, classicSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, classicSudokuViewModel.Solutions.Count);
        }
    }
}