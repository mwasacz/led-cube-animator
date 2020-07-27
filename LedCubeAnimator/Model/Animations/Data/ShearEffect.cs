using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class ShearEffect : TransformEffect
    {
        public Plane Plane { get; set; }
        public double Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double value = Value(time);

            double tan = Math.Tan(value * Math.PI / 180);

            var matrix = new Matrix3D(
                1,
                Plane == Plane.XY ? tan : 0,
                Plane == Plane.XZ ? tan : 0,
                Plane == Plane.XY || Plane == Plane.XZ ? Center : 0,

                Plane == Plane.YX ? tan : 0,
                1,
                Plane == Plane.YZ ? tan : 0,
                Plane == Plane.YX || Plane == Plane.YZ ? Center : 0,

                Plane == Plane.ZX ? tan : 0,
                Plane == Plane.ZY ? tan : 0,
                1,
                Plane == Plane.ZX || Plane == Plane.ZY ? Center : 0,

                0,
                0,
                0,
                1);

            return getVoxel(Transform(point, matrix), time);
        }
    }
}
