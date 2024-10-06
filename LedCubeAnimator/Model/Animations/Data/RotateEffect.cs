// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class RotateEffect : TransformEffect
    {
        public Axis Axis { get; set; }
        public Point Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double value = Value(time);

            var axis = new Vector3D(
                Axis == Axis.X ? 1 : 0,
                Axis == Axis.Y ? 1 : 0,
                Axis == Axis.Z ? 1 : 0);

            var center = new Point3D(
                Center.X,
                Center.Y,
                Axis == Axis.X ? Center.X : Center.Y);

            var matrix = Matrix3D.Identity;
            matrix.RotateAt(new Quaternion(axis, value), center);

            return getVoxel(Transform(point, matrix), time);
        }
    }
}
