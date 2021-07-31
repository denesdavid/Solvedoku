using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Solvedoku.Classes;

namespace Solvedoku.Converters
{
    class IntegerToJigsawColorConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            if (index > -1)
            {
                return SudokuBoard.GetJigsawColorsAsSolidColorBrushes()[index];
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<SolidColorBrush> brushes = SudokuBoard.GetJigsawColorsAsSolidColorBrushes();
            foreach (SolidColorBrush brush in brushes)
            {
                if (brush.Color == ((SolidColorBrush)value).Color)
                {
                    return brushes.IndexOf(brush);
                }
            }
            return -1;
        }
    }
}