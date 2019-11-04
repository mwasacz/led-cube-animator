using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class RotateEffect : Effect
    {
        public Vector3D Axis { get; set; }
        public double From { get; set; }
        public double To { get; set; }
        public bool Round { get; set; }

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            var angle = From + (To - From) * ((double)time / Duration);
            var transform = new RotateTransform3D(new AxisAngleRotation3D(Axis, angle));
            point = transform.Transform(point);
            if (Round)
            {
                point = new Point3D(Math.Floor(point.X + 0.5), Math.Floor(point.Y + 0.5), Math.Floor(point.Z + 0.5));
            }
            return getVoxel(point, time);
        }
    }
}
