using System.Windows.Media;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class Animation : Group
    {
        public int Size { get; set; } = 1;
        public ColorMode ColorMode { get; set; }
        public Color MonoColor { get; set; }
        public int FrameDuration { get; set; } = 1;
    }
}
