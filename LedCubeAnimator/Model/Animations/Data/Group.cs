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

            var colors = Children
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
                .ToArray();

            return MixColors(colors);
        }

        private Color MixColors(Color[] colors)
        {
            switch (ColorBlendMode)
            {
                case ColorBlendMode.Add:
                    return colors.Aggregate((c1, c2) => c1.Add(c2));
                case ColorBlendMode.Multiply:
                    return colors.Aggregate((c1, c2) => c1.Multiply(c2));
                case ColorBlendMode.Min:
                    return colors.Aggregate((c1, c2) => c1.Min(c2));
                case ColorBlendMode.Max:
                    return colors.Aggregate((c1, c2) => c1.Max(c2));
                case ColorBlendMode.Average:
                    return colors.Aggregate(Colors.Black, (c1, c2) => c1.Add(c2.Multiply((double)1 / colors.Length)));
                default:
                    return Colors.Black; // ToDo: throw new InvalidOperationException("Current ColorBlendMode is invalid");
            }
        }
    }
}
