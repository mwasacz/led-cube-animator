using System;
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
        public List<Tile> Children { get; set; } = new List<Tile>();
        public ColorBlendMode ColorBlendMode { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            time = (End - Start) / RepeatCount * Fraction(time);
            if (Reverse)
            {
                time /= 2;
            }

            var children = Children
                .Where(c => c.Start <= time + 0.5 && (c.End > time - 0.5 || IsInPersistEffectMode(c, time)))
                .GroupBy(c => c.Hierarchy * 2 + (IsInPersistEffectMode(c, time) ? 0 : 1))
                .OrderByDescending(g => g.Key)
                .ToArray();

            return GetVoxel(point, time, 0);

            Color GetVoxel(Point3D p, double t, int level)
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
                return c1.Add(c2);
            }
            else
            {
                return c1.Multiply(c2);
            }
        }

        private bool IsInPersistEffectMode(Tile tile, double time)
        {
            if (tile is Effect effect)
            {
                return effect.PersistEffect && effect.End <= time - 0.5;
            }
            return false;
        }
    }
}
