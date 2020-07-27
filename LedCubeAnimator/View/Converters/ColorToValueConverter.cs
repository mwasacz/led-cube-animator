using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using LedCubeAnimator.Model.Animations.Data;

namespace LedCubeAnimator.View.Converters
{
    public class ColorToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Color)value).GetBrightness();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte b = System.Convert.ToByte(value);
            return new Color
            {
                A = 255,
                R = b,
                G = b,
                B = b
            };
        }
    }
}
