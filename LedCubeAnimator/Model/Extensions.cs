using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.Model
{
    public static class Extensions
    {
        public static Color Multiply(this Color color1, Color color2)
        {
            return new Color
            {
                A = (byte)(color1.A * color2.A / 255),
                R = (byte)(color1.R * color2.R / 255),
                G = (byte)(color1.G * color2.G / 255),
                B = (byte)(color1.B * color2.B / 255)
            };
        }

        public static Color Opaque(this Color color)
        {
            return new Color
            {
                A = 255,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }
    }
}
