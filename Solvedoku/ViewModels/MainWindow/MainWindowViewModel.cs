using Solvedoku.Commands;
using Solvedoku.Views.AboutBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solvedoku.ViewModels.MainWindow
{
    class MainWindowViewModel:ViewModelBase
    {
        #region Fields
       
        #endregion

        #region Properties

        public ICommand ShowAboutCommand { get; set; }

        public ICommand ShowOptionsCommand { get; set; }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            LoadCommands();
        }

        #endregion

        #region Commands

        bool CanShowOptions() => true;

        void ShowOptions()
        {

        }

        bool CanShowAbout() => true;

        void ShowAbout()
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the command properties in the viewmodel.
        /// </summary>
        void LoadCommands()
        {
            ShowOptionsCommand = new ParameterlessCommand(ShowOptions, CanShowOptions);
            ShowAboutCommand = new ParameterlessCommand(ShowAbout, CanShowAbout);
        }
        #endregion
    }
}