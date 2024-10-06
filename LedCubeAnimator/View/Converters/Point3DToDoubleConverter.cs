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
    public class Point3DToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Point3D p)
            {
                switch ((string)parameter)
                {
                    case "X": return p.X;
                    case "Y": return p.Y;
                    case "Z": return p.Z;
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
                    case "X": return new Point3D(d, double.NaN, double.NaN);
                    case "Y": return new Point3D(double.NaN, d, double.NaN);
                    case "Z": return new Point3D(double.NaN, double.NaN, d);
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
