using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class MoveEffect : TransformEffect
    {
        public Axis Axis { get; set; }

        protected override Matrix3D GetTransformMatrix(double value)
        {
            var vector = new Vector3D(
                Axis == Axis.X ? value : 0,
                Axis == Axis.Y ? value : 0,
                Axis == Axis.Z ? value : 0);

            var matrix = Matrix3D.Identity;
            matrix.Translate(vector);

            return matrix;
        }
    }
}
