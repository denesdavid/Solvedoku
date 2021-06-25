using Solvedoku.ViewModels.JigsawSudoku;
using System.Windows.Controls;

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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           // DataContext = new JigsawSudokuViewModel();
        }
    }
}