using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class MoveEffect : TransformEffect
    {
        public Axis Axis { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double value = Value(time);

            var vector = new Vector3D(
                Axis == Axis.X ? value : 0,
                Axis == Axis.Y ? value : 0,
                Axis == Axis.Z ? value : 0);

            var matrix = Matrix3D.Identity;
            matrix.Translate(vector);

            return getVoxel(Transform(point, matrix), time);
        }
    }
}
