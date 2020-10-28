using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LedCubeAnimator.View.Converters
{
    public class PointToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point p)
            {
                switch ((string)parameter)
                {
                    case "X": return p.X;
                    case "Y": return p.Y;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                switch ((string)parameter)
                {
                    case "X": return new Point(d, double.NaN);
                    case "Y": return new Point(double.NaN, d);
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
