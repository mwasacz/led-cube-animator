using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public abstract class TransformEffect : Effect
    {
        public double From { get; set; }
        public double To { get; set; }
        public bool Round { get; set; }

        protected double Value(double time) => From + (To - From) * Fraction(time);

        protected Point3D Transform(Point3D point, Matrix3D matrix)
        {
            matrix.Invert();
            point = matrix.Transform(point);

            if (Round)
            {
                point = new Point3D(Math.Floor(point.X + 0.5), Math.Floor(point.Y + 0.5), Math.Floor(point.Z + 0.5));
            }

            return point;
        }
    }
}
