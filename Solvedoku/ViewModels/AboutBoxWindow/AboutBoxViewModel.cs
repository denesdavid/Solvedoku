using System;
using System.Reflection;

namespace Solvedoku.ViewModels.AboutBoxWindow
{
    class AboutBoxViewModel : ViewModelBase
    {
        #region Properties

        public string Title => Assembly.GetExecutingAssembly().GetName().Name;
      
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string Description => 
            "Remélem elnyerte tetszésed ez a kis program, mellyel különböző Sudoku feladványokat" +
            " (klasszikus: 4x4, 6x6, 9x9, puzzle: 9x9) oldhatsz meg." +
            " Habár módosítottam pár dolgot a megoldó logikán, illetve refaktoráltam a forráskódot," +
            " az alap verzió nem az én szellemi termékem." +
            " A szoftver ingyenes, viszont arra szeretnélek megkérni, hogy ha felhasználod az egészet, vagy egy részét," +
            " beleértve ebbe a forráskódot is, tüntesd fel a nevemet, illetve, egy, a GitHub repositoryra vezető linket.";

        #endregion
    }
}