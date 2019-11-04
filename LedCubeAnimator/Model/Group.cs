﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class Group : Effect
    {
        public Tile[] Children { get; set; }
        public ColorBlendMode ColorBlendMode { get; set; }

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            var children = Children.GroupBy(c => c.Hierarchy).OrderBy(g => g.Key).ToArray();

            return GetVoxel(point, time, 0);

            Color GetVoxel(Point3D p, int t, int level)
            {
                if (level == children.Length)
                {
                    return getVoxel(p, t);
                }

                return children[level]
                    .Select(c => c.GetVoxel(p, t, (p1, t1) => GetVoxel(p1, t1, level + 1)))
                    .Aggregate(MixColors);
            }
        }

        private Color MixColors(Color c1, Color c2)
        {
            if (ColorBlendMode == ColorBlendMode.Add)
            {
                return c1 + c2;
            }
            else
            {
                return Color.FromArgb(
                    (byte)(c1.A * c2.A / 255),
                    (byte)(c1.R * c2.R / 255),
                    (byte)(c1.G * c2.G / 255),
                    (byte)(c1.B * c2.B / 255));
            }
        }
    }

    public enum ColorBlendMode { Add, Multiply };
}
