using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace Solvedoku.Views.JigsawSudoku
{
    /// <summary>
    /// Interaction logic for UcJigsawSudoku.xaml
    /// </summary>
    public partial class UcJigsawSudoku : UserControl
    {
        #region Constructor

        public UcJigsawSudoku()
        {
            InitializeComponent();
        }

        #endregion

        private void ColorPicker_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            ((ColorPicker)sender).GetBindingExpression(ColorPicker.StandardColorsProperty).UpdateTarget();
        }
    }
}