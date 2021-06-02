using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Solvedoku.Classes;

namespace Solvedoku.Converters
{
    class IntegerToPuzzleColorConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            if (index > -1)
            {
                return SudokuBoard.GetPuzzleColorsAsSolidColorBrushes()[index];
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<SolidColorBrush> brushes = SudokuBoard.GetPuzzleColorsAsSolidColorBrushes();
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