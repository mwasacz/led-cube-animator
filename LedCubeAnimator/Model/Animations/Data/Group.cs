using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class Group : Effect
    {
        public List<Tile> Children { get; set; } = new List<Tile>();
        public ColorBlendMode ColorBlendMode { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            time = GetLength() * Fraction(time);
            time /= Reverse ? RepeatCount * 2 : RepeatCount;

            return Children
                .GroupBy(tile => tile.Channel)
                .Select(channel => channel
                    .GroupBy(tile => tile.Hierarchy)
                    .OrderBy(group => group.Key)
                    .Aggregate(getVoxel, (func, group) => (p, t) => group
                        .FirstOrDefault(tile => t >= tile.Start && t < tile.End + 1) // ToDo: SingleOrDefault
                        ?.GetVoxel(p, t, func)
                        ?? func(p, t)))
                .DefaultIfEmpty(getVoxel)
                .Select(func => func(point, time))
                .Aggregate(MixColors);
        }

        private Color MixColors(Color c1, Color c2)
        {
            switch (ColorBlendMode)
            {
                case ColorBlendMode.Add:
                    return c1.Add(c2);
                case ColorBlendMode.Multiply:
                    return c1.Multiply(c2);
                case ColorBlendMode.Min:
                    return c1.Min(c2);
                case ColorBlendMode.Max:
                    return c1.Max(c2);
                case ColorBlendMode.Average:
                    return c1.Average(c2);
                default:
                    throw new Exception(); // ToDo: NotSupportedException
            }
        }
    }
}
