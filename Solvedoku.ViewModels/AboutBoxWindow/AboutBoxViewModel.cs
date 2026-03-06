using System.Reflection;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.ViewModels.AboutBoxWindow
{
    class AboutBoxViewModel : ViewModelBase
    {
        #region Properties

        public string Title => Assembly.GetExecutingAssembly().GetName().Name;
      
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region Constructor

        public AboutBoxViewModel(IMessageBoxService messageBoxService):base(messageBoxService){}

        #endregion
    }
}