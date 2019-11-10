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
        public Point3D Center { get; set; }
        public Vector3D From { get; set; }
        public Vector3D To { get; set; }
        public bool Round { get; set; }

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            var angle = From + (To - From) * (time - Start) / (End - Start);
            var matrix = Matrix3D.Identity;
            matrix.RotateAt(new Quaternion(new Vector3D(1, 0, 0), angle.X), Center);
            matrix.RotateAt(new Quaternion(new Vector3D(0, 1, 0), angle.Y), Center);
            matrix.RotateAt(new Quaternion(new Vector3D(0, 0, 1), angle.Z), Center);
            matrix.Invert();
            point = matrix.Transform(point);
            if (Round)
            {
                point = new Point3D(Math.Floor(point.X + 0.5), Math.Floor(point.Y + 0.5), Math.Floor(point.Z + 0.5));
            }
            return getVoxel(point, time);
        }
    }
}
