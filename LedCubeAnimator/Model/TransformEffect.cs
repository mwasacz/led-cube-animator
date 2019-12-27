using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public abstract class TransformEffect : Effect
    {
        public double From { get; set; }
        public double To { get; set; }
        public bool Round { get; set; }

        protected abstract Matrix3D GetTransformMatrix(double value);

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            double value = From + (To - From) * (time - Start) / (End - Start);

            var matrix = GetTransformMatrix(value);
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
