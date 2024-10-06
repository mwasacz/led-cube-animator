// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using LedCubeAnimator.Utils;
using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class GradientEffect : Effect
    {
        public Color From { get; set; } = Colors.Black;
        public Color To { get; set; } = Colors.Black;
        public ColorInterpolation ColorInterpolation { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double fraction = Fraction(time);

            if (ColorInterpolation == ColorInterpolation.RGB)
            {
                return new Color
                {
                    A = 255,
                    R = (byte)(From.R + (To.R - From.R) * fraction),
                    G = (byte)(From.G + (To.G - From.G) * fraction),
                    B = (byte)(From.B + (To.B - From.B) * fraction)
                };
            }
            else
            {
                var from = From.ToHsv();
                var to = To.ToHsv();

                double d1 = to.H - from.H + (to.H < from.H ? 360 : 0);
                double d2 = from.H - to.H + (from.H <= to.H ? 360 : 0);
                double d = (d1 < d2) == (ColorInterpolation == ColorInterpolation.HSV) ? d1 : -d2;

                double h = (from.H + d * fraction + 360) % 360;
                double s = from.S + (to.S - from.S) * fraction;
                double v = from.V + (to.V - from.V) * fraction;

                var result = new HsvColor(h, s, v);
                return result.ToRgb();
            }
        }
    }
}
