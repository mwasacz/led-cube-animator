﻿using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class LinearDelay : Delay
    {
        public Axis Axis { get; set; }

        public double Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double distance = 0;

            switch (Axis)
            {
                case Axis.X:
                    distance = point.X - Center;
                    break;
                case Axis.Y:
                    distance = point.Y - Center;
                    break;
                case Axis.Z:
                    distance = point.Z - Center;
                    break;
            }

            return getVoxel(point, GetDelayedTime(time, distance));
        }
    }
}
