using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class LinearDelayEffect : Effect
    {
        public Axis Axis { get; set; }

        public double Center { get; set; }

        public double Value { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double d = 0;

            switch (Axis)
            {
                case Axis.X: d = point.X; break;
                case Axis.Y: d = point.Y; break;
                case Axis.Z: d = point.Z; break;
            }

            d -= Center;

            return getVoxel(point, time - d * Value);
        }
    }
}
