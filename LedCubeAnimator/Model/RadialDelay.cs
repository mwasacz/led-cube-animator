﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class RadialDelay : Delay
    {
        public Axis Axis { get; set; }

        public Point Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            var p = new Point(
                Axis == Axis.X ? point.Z : point.X,
                Axis == Axis.Y ? point.Z : point.Y);

            var distance = (p - Center).Length;

            return getVoxel(point, GetDelayedTime(time, distance));
        }
    }
}
