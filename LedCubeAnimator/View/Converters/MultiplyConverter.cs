using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace LedCubeAnimator.View.Converters
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Select(System.Convert.ToDouble).Aggregate((double)1, (x, y) => x * y);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
