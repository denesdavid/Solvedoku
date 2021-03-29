using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solvedoku.Views.BusyWindow
{
    class WindowExtensions : DependencyObject
    {
        public static readonly DependencyProperty IsBusyProperty =
        DependencyProperty.RegisterAttached(
                    "IsBusy",
                    typeof(bool),
                    typeof(BusyWindowControl),
                    new PropertyMetadata(false, OnIsBusyChanged));

        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
                
            }
        }

        static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if ((bool)e.NewValue)
            {
                window.ShowDialog();
            }
        }
    }
}