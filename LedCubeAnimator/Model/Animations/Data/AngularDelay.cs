// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class AngularDelay : Delay
    {
        public Axis Axis { get; set; }

        public Point Center { get; set; }

        public double StartAngle { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            var p = new Point(
                Axis == Axis.X ? point.Z : point.X,
                Axis == Axis.Y ? point.Z : point.Y);

            var v = p - Center;

            var angle = (Math.Atan2(v.Y, -v.X) / Math.PI - 1) / -2 - StartAngle / 360;

            return getVoxel(point, GetDelayedTime(time, (angle % 1 + 1) % 1));
        }
    }
}
