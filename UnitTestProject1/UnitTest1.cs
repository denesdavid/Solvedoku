using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Solvedoku.Classes;
using Solvedoku.Services.MessageBox;
using Solvedoku.ViewModels;
using Solvedoku.ViewModels.ClassicSudoku;
using Solvedoku.Views.ClassicSudoku;
using System;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<IMessageBoxService> messageBoxMock = new Mock<IMessageBoxService>();
            messageBoxMock.Setup(m => m.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
                .Returns(MessageBoxResult.Yes);
            // messageBoxMock.SetReturnsDefault(MessageBoxResult.OK);

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
