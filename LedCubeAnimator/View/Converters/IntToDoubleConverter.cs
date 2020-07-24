using System;
using System.Globalization;
using System.Windows.Data;

namespace LedCubeAnimator.View.Converters
{
    public class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value + (double)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((double)value - (double)parameter);
        }
    }
}
