using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solvedoku.Classes;
using Solvedoku.ViewModels.JigsawSudoku;
using Solvedoku.Views.JigsawSudoku;
using System;

namespace Solvedoku.Tests.ViewModelsTests
{
    [TestClass]
    public class JigsawSudokuViewModelTest
    {
        [TestMethod]
        public void Draw9x9TableTest()
        {
            JigsawSudokuViewModel jigsawSudokuViewModel = new JigsawSudokuViewModel();
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 9;
            sudokuBoardSize.Width = 9;
            sudokuBoardSize.BoxCountX = 3;
            sudokuBoardSize.BoxCountY = 3;

            jigsawSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);
            object expectedBoard = jigsawSudokuViewModel.SudokuBoardControl;

            Assert.AreEqual(typeof(UcJigsawSudoku9x9Table), expectedBoard.GetType());
            Assert.AreEqual(string.Empty, jigsawSudokuViewModel.SolutionCounter);
            Assert.AreEqual(false, jigsawSudokuViewModel.IsSolutionCounterVisible);
            Assert.AreEqual(0, jigsawSudokuViewModel.Solutions.Count);
        }

        [TestMethod]
        public void Solve9x9TableWithPredefinedCellsTest()
        {
            JigsawSudokuViewModel jigsawSudokuViewModel = new JigsawSudokuViewModel();
            SudokuBoardSize sudokuBoardSize = new SudokuBoardSize();
            sudokuBoardSize.Height = 9;
            sudokuBoardSize.Width = 9;
            sudokuBoardSize.BoxCountX = 3;
            sudokuBoardSize.BoxCountY = 3;

            jigsawSudokuViewModel.DrawSudokuCommand.Execute(sudokuBoardSize);


            object expectedBoard = jigsawSudokuViewModel.SudokuBoardControl;
            var tableViewModel = (BaseJigsawSudokuTableViewModel)jigsawSudokuViewModel.GetCurrentTableViewModel();
            tableViewModel.Cells[0][0] = "5";
            tableViewModel.Cells[0][8] = "2";
            tableViewModel.Cells[1][1] = "8";
            tableViewModel.Cells[1][7] = "6";
            tableViewModel.Cells[2][4] = "1";
            tableViewModel.Cells[3][3] = "8";
            tableViewModel.Cells[3][5] = "2";
            tableViewModel.Cells[4][2] = "7";
            tableViewModel.Cells[4][6] = "3";
            tableViewModel.Cells[5][3] = "6";
            tableViewModel.Cells[5][5] = "4";
            tableViewModel.Cells[6][4] = "5";
            tableViewModel.Cells[7][1] = "5";
            tableViewModel.Cells[7][7] = "4";
            tableViewModel.Cells[8][0] = "2";
            tableViewModel.Cells[8][8] = "7";

            string[] areas = new string[9] { "011122222",
                                             "001112232",
                                             "000112333",
                                             "800414333",
                                             "804444453",
                                             "888464553",
                                             "888766555",
                                             "787766655",
                                             "777776665" };
            jigsawSudokuViewModel.ActualSudokuBoard = jigsawSudokuViewModel.CreateBoard(sudokuBoardSize, areas, tableViewModel, true);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    tableViewModel.JigsawAreas[i][j] = int.Parse(areas[i][j].ToString());
                }
            }

            jigsawSudokuViewModel.CountOneSolution();
            jigsawSudokuViewModel.DisplaySolutionAndMessage();

            tableViewModel = (BaseJigsawSudokuTableViewModel)jigsawSudokuViewModel.GetCurrentTableViewModel();
            Assert.AreEqual(typeof(UcJigsawSudoku9x9Table), expectedBoard.GetType());
            Assert.AreEqual(0, tableViewModel.JigsawAreas[0][0]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][1]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][2]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][3]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][4]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][5]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][6]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][7]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][8]);

            Assert.AreEqual(0, tableViewModel.JigsawAreas[1][0]);
            Assert.AreEqual(0, tableViewModel.JigsawAreas[1][1]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[1][2]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[1][3]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[1][4]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[1][5]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[1][6]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[1][7]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[1][8]);

            Assert.AreEqual(0, tableViewModel.JigsawAreas[2][0]);
            Assert.AreEqual(0, tableViewModel.JigsawAreas[2][1]);
            Assert.AreEqual(0, tableViewModel.JigsawAreas[2][2]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[2][3]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[2][4]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[2][5]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[2][6]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[2][7]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[2][8]);

            Assert.AreEqual(8, tableViewModel.JigsawAreas[3][0]);
            Assert.AreEqual(0, tableViewModel.JigsawAreas[3][1]);
            Assert.AreEqual(0, tableViewModel.JigsawAreas[3][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[3][3]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[3][4]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[3][5]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[3][6]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[3][7]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[3][8]);

            Assert.AreEqual(8, tableViewModel.JigsawAreas[4][0]);
            Assert.AreEqual(0, tableViewModel.JigsawAreas[4][1]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[4][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[4][3]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[4][4]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[4][5]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[4][6]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[4][7]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[4][8]);

            Assert.AreEqual(8, tableViewModel.JigsawAreas[5][0]);
            Assert.AreEqual(8, tableViewModel.JigsawAreas[5][1]);
            Assert.AreEqual(8, tableViewModel.JigsawAreas[5][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[5][3]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[5][4]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[5][5]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[5][6]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[5][7]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[5][8]);

            Assert.AreEqual(8, tableViewModel.JigsawAreas[6][0]);
            Assert.AreEqual(8, tableViewModel.JigsawAreas[6][1]);
            Assert.AreEqual(8, tableViewModel.JigsawAreas[6][2]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[6][3]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[6][4]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[6][5]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[6][6]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[6][7]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[6][8]);

            Assert.AreEqual(7, tableViewModel.JigsawAreas[7][0]);
            Assert.AreEqual(8, tableViewModel.JigsawAreas[7][1]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[7][2]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[7][3]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[7][4]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[7][5]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[7][6]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[7][7]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[7][8]);

            Assert.AreEqual(7, tableViewModel.JigsawAreas[8][0]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[8][1]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[8][2]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[8][3]);
            Assert.AreEqual(7, tableViewModel.JigsawAreas[8][4]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[8][5]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[8][6]);
            Assert.AreEqual(6, tableViewModel.JigsawAreas[8][7]);
            Assert.AreEqual(5, tableViewModel.JigsawAreas[8][8]);

            Assert.AreEqual("5", tableViewModel.Cells[0][0]);
            Assert.AreEqual("3", tableViewModel.Cells[0][1]);
            Assert.AreEqual("8", tableViewModel.Cells[0][2]);
            Assert.AreEqual("9", tableViewModel.Cells[0][3]);
            Assert.AreEqual("4", tableViewModel.Cells[0][4]);
            Assert.AreEqual("1", tableViewModel.Cells[0][5]);
            Assert.AreEqual("6", tableViewModel.Cells[0][6]);
            Assert.AreEqual("7", tableViewModel.Cells[0][7]);
            Assert.AreEqual("2", tableViewModel.Cells[0][8]);

            Assert.AreEqual("1", tableViewModel.Cells[1][0]);
            Assert.AreEqual("8", tableViewModel.Cells[1][1]);
            Assert.AreEqual("4", tableViewModel.Cells[1][2]);
            Assert.AreEqual("2", tableViewModel.Cells[1][3]);
            Assert.AreEqual("7", tableViewModel.Cells[1][4]);
            Assert.AreEqual("3", tableViewModel.Cells[1][5]);
            Assert.AreEqual("9", tableViewModel.Cells[1][6]);
            Assert.AreEqual("6", tableViewModel.Cells[1][7]);
            Assert.AreEqual("5", tableViewModel.Cells[1][8]);

            Assert.AreEqual("3", tableViewModel.Cells[2][0]);
            Assert.AreEqual("4", tableViewModel.Cells[2][1]);
            Assert.AreEqual("6", tableViewModel.Cells[2][2]);
            Assert.AreEqual("5", tableViewModel.Cells[2][3]);
            Assert.AreEqual("1", tableViewModel.Cells[2][4]);
            Assert.AreEqual("8", tableViewModel.Cells[2][5]);
            Assert.AreEqual("7", tableViewModel.Cells[2][6]);
            Assert.AreEqual("2", tableViewModel.Cells[2][7]);
            Assert.AreEqual("9", tableViewModel.Cells[2][8]);

            Assert.AreEqual("4", tableViewModel.Cells[3][0]);
            Assert.AreEqual("7", tableViewModel.Cells[3][1]);
            Assert.AreEqual("9", tableViewModel.Cells[3][2]);
            Assert.AreEqual("8", tableViewModel.Cells[3][3]);
            Assert.AreEqual("6", tableViewModel.Cells[3][4]);
            Assert.AreEqual("2", tableViewModel.Cells[3][5]);
            Assert.AreEqual("5", tableViewModel.Cells[3][6]);
            Assert.AreEqual("3", tableViewModel.Cells[3][7]);
            Assert.AreEqual("1", tableViewModel.Cells[3][8]);

            Assert.AreEqual("6", tableViewModel.Cells[4][0]);
            Assert.AreEqual("2", tableViewModel.Cells[4][1]);
            Assert.AreEqual("7", tableViewModel.Cells[4][2]);
            Assert.AreEqual("1", tableViewModel.Cells[4][3]);
            Assert.AreEqual("9", tableViewModel.Cells[4][4]);
            Assert.AreEqual("5", tableViewModel.Cells[4][5]);
            Assert.AreEqual("3", tableViewModel.Cells[4][6]);
            Assert.AreEqual("8", tableViewModel.Cells[4][7]);
            Assert.AreEqual("4", tableViewModel.Cells[4][8]);

            Assert.AreEqual("7", tableViewModel.Cells[5][0]);
            Assert.AreEqual("9", tableViewModel.Cells[5][1]);
            Assert.AreEqual("2", tableViewModel.Cells[5][2]);
            Assert.AreEqual("6", tableViewModel.Cells[5][3]);
            Assert.AreEqual("3", tableViewModel.Cells[5][4]);
            Assert.AreEqual("4", tableViewModel.Cells[5][5]);
            Assert.AreEqual("1", tableViewModel.Cells[5][6]);
            Assert.AreEqual("5", tableViewModel.Cells[5][7]);
            Assert.AreEqual("8", tableViewModel.Cells[5][8]);

            Assert.AreEqual("8", tableViewModel.Cells[6][0]);
            Assert.AreEqual("1", tableViewModel.Cells[6][1]);
            Assert.AreEqual("3", tableViewModel.Cells[6][2]);
            Assert.AreEqual("4", tableViewModel.Cells[6][3]);
            Assert.AreEqual("5", tableViewModel.Cells[6][4]);
            Assert.AreEqual("7", tableViewModel.Cells[6][5]);
            Assert.AreEqual("2", tableViewModel.Cells[6][6]);
            Assert.AreEqual("9", tableViewModel.Cells[6][7]);
            Assert.AreEqual("6", tableViewModel.Cells[6][8]);

            Assert.AreEqual("9", tableViewModel.Cells[7][0]);
            Assert.AreEqual("5", tableViewModel.Cells[7][1]);
            Assert.AreEqual("1", tableViewModel.Cells[7][2]);
            Assert.AreEqual("7", tableViewModel.Cells[7][3]);
            Assert.AreEqual("2", tableViewModel.Cells[7][4]);
            Assert.AreEqual("6", tableViewModel.Cells[7][5]);
            Assert.AreEqual("8", tableViewModel.Cells[7][6]);
            Assert.AreEqual("4", tableViewModel.Cells[7][7]);
            Assert.AreEqual("3", tableViewModel.Cells[7][8]);

            Assert.AreEqual("2", tableViewModel.Cells[8][0]);
            Assert.AreEqual("6", tableViewModel.Cells[8][1]);
            Assert.AreEqual("5", tableViewModel.Cells[8][2]);
            Assert.AreEqual("3", tableViewModel.Cells[8][3]);
            Assert.AreEqual("8", tableViewModel.Cells[8][4]);
            Assert.AreEqual("9", tableViewModel.Cells[8][5]);
            Assert.AreEqual("4", tableViewModel.Cells[8][6]);
            Assert.AreEqual("1", tableViewModel.Cells[8][7]);
            Assert.AreEqual("7", tableViewModel.Cells[8][8]);
        }

        [TestMethod]
        public void LoadDeserializedJigsawSudokuFileTest()
        {

        }

        [TestMethod]
        public void DisplayAreasTest()
        {
            int[,] testMatrix = new int[4, 4] { { 1, 2, 3, 4 },
                                                { 1, 2, 3, 4 },
                                                { 1, 2, 3, 4 },
                                                { 1, 2, 3, 4 } };

            JigsawSudokuViewModel jigsawSudokuViewModel = new JigsawSudokuViewModel();
            jigsawSudokuViewModel.DisplayAreas(testMatrix);
            var tableViewModel = (BaseJigsawSudokuTableViewModel)jigsawSudokuViewModel.GetCurrentTableViewModel();
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][0]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][1]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[0][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[0][3]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][0]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][1]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[0][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[0][3]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][0]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][1]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[0][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[0][3]);
            Assert.AreEqual(1, tableViewModel.JigsawAreas[0][0]);
            Assert.AreEqual(2, tableViewModel.JigsawAreas[0][1]);
            Assert.AreEqual(3, tableViewModel.JigsawAreas[0][2]);
            Assert.AreEqual(4, tableViewModel.JigsawAreas[0][3]);
        }
    }
}