using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class ScaleEffect : TransformEffect
    {
        public Axis Axis { get; set; }
        public double Center { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double value = Value(time);

            var scale = new Vector3D(
                Axis == Axis.X ? value : 1,
                Axis == Axis.Y ? value : 1,
                Axis == Axis.Z ? value : 1);

            var center = new Point3D(
                Axis == Axis.X ? Center : 0,
                Axis == Axis.Y ? Center : 0,
                Axis == Axis.Z ? Center : 0);

            var matrix = Matrix3D.Identity;
            matrix.ScaleAt(scale, center);

            return getVoxel(Transform(point, matrix), time);
        }
    }
}
