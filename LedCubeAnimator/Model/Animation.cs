using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.Model
{
    public class Animation
    {
        public Group MainGroup { get; set; }
        public int Size { get; set; }

        private ColorMode _colorMode;
        public ColorMode ColorMode
        {
            get => _colorMode;
            set
            {
                if (_colorMode != value)
                {
                    _colorMode = value;
                    if (_colorMode != ColorMode.RGB)
                    {
                        SetGroupColorMode(MainGroup);
                    }
                }
            }
        }

        private void SetGroupColorMode(Group group)
        {
            foreach (var t in group.Children)
            {
                switch (t)
                {
                    case Frame fr:
                        for (int x = 0; x < Size; x++)
                        {
                            for (int y = 0; y < Size; y++)
                            {
                                for (int z = 0; z < Size; z++)
                                {
                                    Color oldColor = fr.Voxels[x, y, z];
                                    switch (ColorMode)
                                    {
                                        case ColorMode.Mono:
                                            fr.Voxels[x, y, z] = oldColor.GetBrightness() > 127 ? Colors.White : Colors.Black;
                                            break;
                                        case ColorMode.MonoBrightness:
                                            fr.Voxels[x, y, z] = Colors.White.Multiply(oldColor.GetBrightness()).Opaque();
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    case Group gr:
                        SetGroupColorMode(gr);
                        break;
                }
            }
        }

        public Color MonoColor { get; set; }
        public int FrameDuration { get; set; }
    }
}
