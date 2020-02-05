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
        public static Color Add(this Color color1, Color color2)
        {
            return new Color
            {
                A = (byte)Math.Min(color1.A + color2.A, 255),
                R = (byte)Math.Min(color1.R + color2.R, 255),
                G = (byte)Math.Min(color1.G + color2.G, 255),
                B = (byte)Math.Min(color1.B + color2.B, 255)
            };
        }

        public static Color Subtract(this Color color1, Color color2)
        {
            return new Color
            {
                A = (byte)Math.Min(color1.A - color2.A, 255),
                R = (byte)Math.Min(color1.R - color2.R, 255),
                G = (byte)Math.Min(color1.G - color2.G, 255),
                B = (byte)Math.Min(color1.B - color2.B, 255)
            };
        }

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

        public static Color Multiply(this Color color, double brightness)
        {
            return new Color
            {
                A = (byte)(color.A * brightness),
                R = (byte)(color.R * brightness),
                G = (byte)(color.G * brightness),
                B = (byte)(color.B * brightness)
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

        public static byte GetBrightness(this Color color)
        {
            return Math.Max(Math.Max(color.R, color.G), color.B);
        }
    }
}
