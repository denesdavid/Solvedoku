using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.ViewModels
{
    /// <summary>
    /// Base class for ViewModels.
    /// </summary>
    class ViewModelBase : INotifyPropertyChanged
    {
        #region Fields
        public IMessageBoxService MessageBoxService = new MessageBoxService();
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}