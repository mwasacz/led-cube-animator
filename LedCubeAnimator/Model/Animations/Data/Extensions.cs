using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.Model.Animations.Data
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

        public static Color Min(this Color color1, Color color2)
        {
            return new Color
            {
                A = Math.Min(color1.A, color2.A),
                R = Math.Min(color1.R, color2.R),
                G = Math.Min(color1.G, color2.G),
                B = Math.Min(color1.B, color2.B)
            };
        }

        public static Color Max(this Color color1, Color color2)
        {
            return new Color
            {
                A = Math.Max(color1.A, color2.A),
                R = Math.Max(color1.R, color2.R),
                G = Math.Max(color1.G, color2.G),
                B = Math.Max(color1.B, color2.B)
            };
        }

        public static Color Average(this Color color1, Color color2)
        {
            return new Color
            {
                A = (byte)((color1.A + color2.A) / 2),
                R = (byte)((color1.R + color2.R) / 2),
                G = (byte)((color1.G + color2.G) / 2),
                B = (byte)((color1.B + color2.B) / 2)
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

        public static Color Multiply(this Color color1, byte brightness)
        {
            return new Color
            {
                A = (byte)(color1.A * brightness / 255),
                R = (byte)(color1.R * brightness / 255),
                G = (byte)(color1.G * brightness / 255),
                B = (byte)(color1.B * brightness / 255)
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
