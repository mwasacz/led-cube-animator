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

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            var children = Children.Where(c => c.Start <= time && c.End >= time).GroupBy(c => c.Hierarchy).OrderBy(g => g.Key).ToArray();

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
                return c1.Multiply(c2);
            }
        }
    }
}
