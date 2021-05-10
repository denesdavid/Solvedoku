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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solvedoku.Views.TabControlWithRightContentArea
{
    /// <summary>
    /// Interaction logic for UcTabControlWithRightContentArea.xaml
    /// </summary>
    public partial class UcTabControlWithRightContentArea : UserControl
    {
        public UcTabControlWithRightContentArea()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty RightContent =

DependencyProperty.Register("RightContentArea",

typeof(Control),

typeof(UcTabControlWithRightContentArea));

        public Control RightContentArea
        {
            get
            {
                return GetValue(RightContent) as Control;
            }
            set
            {
                SetValue(RightContent, value);
            }
        }
    }
}
