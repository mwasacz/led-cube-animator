// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using LedCubeAnimator.Utils;

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
