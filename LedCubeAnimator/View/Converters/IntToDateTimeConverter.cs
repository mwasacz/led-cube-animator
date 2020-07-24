﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace LedCubeAnimator.View.Converters
{
    public class IntToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new DateTime(Math.Max((int)value, 0) + (int)parameter + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)((DateTime)value).Ticks - (int)parameter - 1;
        }
    }
}
