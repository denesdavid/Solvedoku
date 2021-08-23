using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Solvedoku.ViewModels.ClassicSudoku;
using Solvedoku.Classes;
using Solvedoku.Views.ClassicSudoku;

namespace Solvedoku.Tests.ViewModelsTests
{
    /// <summary>
    /// Summary description for ClassicSudokuViewModelTest
    /// </summary>
    [TestClass]
    public class ClassicSudokuViewModelTest
    {
        public ClassicSudokuViewModelTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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

    }
}
