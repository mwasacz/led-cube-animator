using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public abstract class Tile
    {
        public string Name { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Hierarchy { get; set; }

        public abstract Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel);
    }
}
