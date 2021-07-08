using System;
using System.Globalization;
using System.Windows.Data;

namespace LedCubeAnimator.View.Converters
{
    public class OffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) + (double)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) - (double)parameter;
        }
    }
}
