using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class AngularDelay : Delay
    {
        public Axis Axis { get; set; }

        public Point Center { get; set; }

        public double StartAngle { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            var p = new Point(
                Axis == Axis.X ? point.Z : point.X,
                Axis == Axis.Y ? point.Z : point.Y);

            var v = p - Center;

            var angle = (Math.Atan2(v.Y, -v.X) / Math.PI - 1) / -2 - StartAngle / 360;

            return getVoxel(point, GetDelayedTime(time, (angle % 1 + 1) % 1));
        }
    }
}
