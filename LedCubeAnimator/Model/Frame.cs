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
        public VoxelRoundMode VoxelRoundMode { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            if (VoxelRoundMode == VoxelRoundMode.Round)
            {
                if (IsInCube(point))
                {
                    return GetSingleVoxel(new Point3D(Math.Floor(point.X + 0.5), Math.Floor(point.Y + 0.5), Math.Floor(point.Z + 0.5)));
                }
                else
                {
                    return getVoxel(point, time);
                }
            }
            else
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

                double weightSum = 0;
                Color color = new Color();

                foreach (var p in points.Distinct().Where(IsInCube))
                {
                    double weight = (1 - Math.Abs(p.X - point.X)) * (1 - Math.Abs(p.Y - point.Y)) * (1 - Math.Abs(p.Z - point.Z));
                    weightSum += weight;
                    color = MixColors(color, GetSingleVoxel(p), weight);
                }

                if (!IsInCube(point))
                {
                    color = MixColors(color, getVoxel(point, time), 1 - weightSum);
                }

                return color.Opaque(); // ToDo: is Opaque necessary ?
            }
        }

        private bool IsInCube(Point3D p) =>
            p.X >= Offset.X
            && p.Y >= Offset.Y
            && p.Z >= Offset.Z
            && p.X <= Offset.X + Voxels.GetLength(0) - 1
            && p.Y <= Offset.Y + Voxels.GetLength(1) - 1
            && p.Z <= Offset.Z + Voxels.GetLength(2) - 1;

        private Color GetSingleVoxel(Point3D p) => Voxels[(int)(p.X - Offset.X), (int)(p.Y - Offset.Y), (int)(p.Z - Offset.Z)];

        private Color MixColors(Color baseColor, Color color, double weight)
        {
            switch (VoxelRoundMode)
            {
                case VoxelRoundMode.Average:
                    return baseColor.Add(color.Multiply(weight));
                case VoxelRoundMode.Min:
                    return baseColor.Min(color);
                case VoxelRoundMode.Max:
                    return baseColor.Max(color);
                default:
                    throw new Exception(); // ToDo
            }
        }
    }
}
