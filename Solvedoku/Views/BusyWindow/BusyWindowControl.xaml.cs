using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Solvedoku.Views.BusyWindow
{
    /// <summary>
    /// Interaction logic for BusyWindow.xaml
    /// </summary>
    public partial class BusyWindowControl : Window
    {
        public BusyWindowControl()
        {
            InitializeComponent();
            
        }

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