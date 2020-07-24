﻿using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public abstract class Tile
    {
        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Channel { get; set; }
        public int Hierarchy { get; set; }

        public abstract Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel);

        protected int GetLength() => End - Start + 1;
    }
}
