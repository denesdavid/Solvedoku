using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Solvedoku.CustomControls.TabControlWithRightContentArea
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Solvedoku.CustomControls.TabControlWithRightContentArea"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Solvedoku.CustomControls.TabControlWithRightContentArea;assembly=Solvedoku.CustomControls.TabControlWithRightContentArea"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TabControlWithRightContentArea/>
    ///
    /// </summary>
    
    public class TabControlWithRightContentArea : TabControl
    {
        public static DependencyProperty RightAreaContentProperty;

        static TabControlWithRightContentArea()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControlWithRightContentArea), new FrameworkPropertyMetadata(typeof(TabControlWithRightContentArea)));
            RightAreaContentProperty = DependencyProperty.Register("RightAreaContent", typeof(object), typeof(TabControlWithRightContentArea));
        }

        public object RightAreaContent
        {
            get => GetValue(RightAreaContentProperty);
            set => SetValue(RightAreaContentProperty, value);
        }
    }
}
