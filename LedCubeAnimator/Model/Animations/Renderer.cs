using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Utils;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations
{
    public static class Renderer
    {
        public static Color[,,] Render(Animation animation, Tile tile, int frame, bool preview)
        {
            double time = tile.Start + frame + 0.5;
            int size = animation.Size;
            var voxels = new Color[size, size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        var color = tile.GetVoxel(new Point3D(x, y, z), time, (p, t) => Colors.Black);
                        if (preview && animation.ColorMode != ColorMode.RGB)
                        {
                            color = color.Multiply(animation.MonoColor);
                        }
                        voxels[x, y, z] = color;
                    }
                }
            }
            return voxels;
        }

        public static Color[,,] Render(Animation animation, int frame, bool preview) => Render(animation, animation, frame, preview);
    }
}
