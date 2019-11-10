using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class MoveEffect : Effect
    {
        public Vector3D From { get; set; }
        public Vector3D To { get; set; }
        public bool Round { get; set; }

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            var vector = From + (To - From) * (time - Start) / (End - Start);
            point -= vector;
            //var matrix = Matrix3D.Identity;
            //matrix.Translate(vector);
            //matrix.Invert();
            //point = matrix.Transform(point);
            if (Round)
            {
                point = new Point3D(Math.Floor(point.X + 0.5), Math.Floor(point.Y + 0.5), Math.Floor(point.Z + 0.5));
            }
            return getVoxel(point, time);
        }
    }
}
