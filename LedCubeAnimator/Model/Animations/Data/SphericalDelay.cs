using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class SphericalDelay : Delay
    {
        public Point3D Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            var distance = (point - Center).Length;

            return getVoxel(point, GetDelayedTime(time, distance));
        }
    }
}
