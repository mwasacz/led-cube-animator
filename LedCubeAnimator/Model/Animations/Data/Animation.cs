using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class Animation
    {
        public Group MainGroup { get; set; }
        public int Size { get; set; }
        public ColorMode ColorMode { get; set; }
        public Color MonoColor { get; set; }
        public int FrameDuration { get; set; }
    }
}
