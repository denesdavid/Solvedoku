﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Solvedoku.Services.MessageBox;

namespace Solvedoku.ViewModels
{
    /// <summary>
    /// Base class for ViewModels.
    /// </summary>
    class ViewModelBase : INotifyPropertyChanged
    {
        #region Fields
        public static IMessageBoxService MessageBoxService = new MessageBoxService();
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