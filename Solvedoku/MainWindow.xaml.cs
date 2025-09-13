using System.Windows;

namespace Solvedoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            MinHeight = Height;
            MinWidth = Width;
        }

        #endregion
    }
}