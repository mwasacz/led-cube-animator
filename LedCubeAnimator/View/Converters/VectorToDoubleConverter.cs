using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LedCubeAnimator.View.Converters
{
    public class VectorToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector v)
            {
                switch ((string)parameter)
                {
                    case "X": return v.X;
                    case "Y": return v.Y;
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
                    case "X": return new Vector(d, double.NaN);
                    case "Y": return new Vector(double.NaN, d);
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
