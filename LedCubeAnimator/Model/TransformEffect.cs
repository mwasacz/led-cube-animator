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

        protected double Value(double time) => From + (To - From) * Fraction(time);

        protected Point3D Transform(Point3D point, Matrix3D matrix)
        {
            matrix.Invert();
            return matrix.Transform(point);
        }
    }
}
