// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.View.Converters
{
    public class Vector3DToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Vector3D v)
            {
                switch ((string)parameter)
                {
                    case "X": return v.X;
                    case "Y": return v.Y;
                    case "Z": return v.Z;
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
                    case "X": return new Vector3D(d, double.NaN, double.NaN);
                    case "Y": return new Vector3D(double.NaN, d, double.NaN);
                    case "Z": return new Vector3D(double.NaN, double.NaN, d);
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
