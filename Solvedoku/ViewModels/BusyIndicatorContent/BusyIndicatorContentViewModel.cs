using Solvedoku.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Solvedoku.ViewModels.BusyIndicatorContent
{
    class BusyIndicatorContentViewModel:ViewModelBase
    {
        #region Fields
        private static BusyIndicatorContentViewModel _instance;
        #endregion

        #region Properties
        public static BusyIndicatorContentViewModel Instance { get => _instance; }
        public ISudokuViewModel SudokuViewModel { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region Commands

        /// <summary>
        /// Determines if cancelling the busy task is possible.
        /// </summary>
        /// <returns>Bool (currently always true)</returns>
        bool CanCancel() => true;

        /// <summary>
        /// Cancels the busy task.
        /// </summary>
        void Cancel()
        {
            var messageBoxResult = MessageBoxService.Show("Biztos, hogy meg szeretnéd szakítani a megoldást?", "Figyelmeztetés!",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SudokuViewModel.SudokuSolverThread.Abort();
                SudokuViewModel.IsBusy = false;
            }
        }
        #endregion

        #region Constructor
        public BusyIndicatorContentViewModel()
        {
            LoadCommands();
        }
        #endregion

        #region Methods
        void LoadCommands()
        {
            _instance = this;
            CancelCommand = new ParameterlessCommand(Cancel, CanCancel);
        }
        #endregion
    }
}