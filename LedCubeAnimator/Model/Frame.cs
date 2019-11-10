using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public class Frame : Tile
    {
        public Point3D Offset { get; set; }
        public Color[,,] Voxels { get; set; } = new Color[0, 0, 0];

        public override Color GetVoxel(Point3D point, int time, Func<Point3D, int, Color> getVoxel)
        {
            var points = new Point3D[] {
                new Point3D(Math.Floor(point.X), Math.Floor(point.Y), Math.Floor(point.Z)),
                new Point3D(Math.Floor(point.X), Math.Floor(point.Y), Math.Ceiling(point.Z)),
                new Point3D(Math.Floor(point.X), Math.Ceiling(point.Y), Math.Floor(point.Z)),
                new Point3D(Math.Floor(point.X), Math.Ceiling(point.Y), Math.Ceiling(point.Z)),
                new Point3D(Math.Ceiling(point.X), Math.Floor(point.Y), Math.Floor(point.Z)),
                new Point3D(Math.Ceiling(point.X), Math.Floor(point.Y), Math.Ceiling(point.Z)),
                new Point3D(Math.Ceiling(point.X), Math.Ceiling(point.Y), Math.Floor(point.Z)),
                new Point3D(Math.Ceiling(point.X), Math.Ceiling(point.Y), Math.Ceiling(point.Z))
            };

            float weightSum = 0;
            Color color = new Color();

            foreach (var p in points.Distinct().Where(IsInCube))
            {
                float weight = (float)((1 - Math.Abs(p.X - point.X)) * (1 - Math.Abs(p.Y - point.Y)) * (1 - Math.Abs(p.Z - point.Z))); // ToDo: do not multiply alpha, multiply by double
                weightSum += weight;
                color += Color.Multiply(GetSingleVoxel(p), weight);
            }

            if (!IsInCube(point))
            {
                color += Color.Multiply(getVoxel(point, time), 1 - weightSum);
            }

            return color;
        }

        private bool IsInCube(Point3D p) =>
            p.X >= Offset.X
            && p.Y >= Offset.Y
            && p.Z >= Offset.Z
            && p.X <= Offset.X + Voxels.GetLength(0) - 1
            && p.Y <= Offset.Y + Voxels.GetLength(1) - 1
            && p.Z <= Offset.Z + Voxels.GetLength(2) - 1;

        private Color GetSingleVoxel(Point3D p) => Voxels[(int)(p.X - Offset.X), (int)(p.Y - Offset.Y), (int)(p.Z - Offset.Z)];
    }
}
