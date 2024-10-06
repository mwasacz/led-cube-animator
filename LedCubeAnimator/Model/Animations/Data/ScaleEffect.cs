// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class ScaleEffect : TransformEffect
    {
        public Axis Axis { get; set; }
        public double Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double value = Value(time);

            var scale = new Vector3D(
                Axis == Axis.X ? value : 1,
                Axis == Axis.Y ? value : 1,
                Axis == Axis.Z ? value : 1);

            var center = new Point3D(
                Axis == Axis.X ? Center : 0,
                Axis == Axis.Y ? Center : 0,
                Axis == Axis.Z ? Center : 0);

            var matrix = Matrix3D.Identity;
            matrix.ScaleAt(scale, center);

            return matrix.HasInverse ? getVoxel(Transform(point, matrix), time) : Colors.Black;
        }
    }
}
