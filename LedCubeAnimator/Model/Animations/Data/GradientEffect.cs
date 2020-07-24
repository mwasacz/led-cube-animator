using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class GradientEffect : Effect
    {
        public Color From { get; set; } = Colors.Black;
        public Color To { get; set; } = Colors.Black;
        public ColorInterpolation ColorInterpolation { get; set; }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double fraction = Fraction(time);

            if (ColorInterpolation == ColorInterpolation.RGB)
            {
                return new Color
                {
                    A = 255,
                    R = (byte)(From.R + (To.R - From.R) * fraction),
                    G = (byte)(From.G + (To.G - From.G) * fraction),
                    B = (byte)(From.B + (To.B - From.B) * fraction)
                };
            }
            else
            {
                ColorToHSV(From, out double fromH, out double fromS, out double fromV);
                ColorToHSV(To, out double toH, out double toS, out double toV);

                double d1 = toH - fromH + (toH < fromH ? 360 : 0);
                double d2 = fromH - toH + (fromH <= toH ? 360 : 0);
                double d = (d1 < d2) == (ColorInterpolation == ColorInterpolation.HSV) ? d1 : -d2;
                double h = (fromH + d * fraction + 360) % 360;

                return ColorFromHSV(
                    h,
                    fromS + (toS - fromS) * fraction,
                    fromV + (toV - fromV) * fraction);
            }
        }

        private static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            double r = (double)color.R / 255;
            double g = (double)color.G / 255;
            double b = (double)color.B / 255;

            double max = Math.Max(Math.Max(r, g), b);
            double min = Math.Min(Math.Min(r, g), b);
            double delta = max - min;

            if (delta == 0)
            {
                hue = 0;
            }
            else if (r == max)
            {
                hue = ((g - b) / delta + 6) % 6 * 60;
            }
            else if (g == max)
            {
                hue = ((b - r) / delta + 2) * 60;
            }
            else
            {
                hue = ((r - g) / delta + 4) * 60;
            }
            saturation = max == 0 ? 0 : delta / max;
            value = max;
        }

        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int i = (int)(hue / 60);
            double f = hue / 60 - i;

            byte v = (byte)(255 * value);
            byte p = (byte)(255 * value * (1 - saturation));
            byte q = (byte)(255 * value * (1 - saturation * f));
            byte t = (byte)(255 * value * (1 - saturation * (1 - f)));

            switch (i)
            {
                case 1:
                    return new Color { A = 255, R = q, G = v, B = p };
                case 2:
                    return new Color { A = 255, R = p, G = v, B = t };
                case 3:
                    return new Color { A = 255, R = p, G = q, B = v };
                case 4:
                    return new Color { A = 255, R = t, G = p, B = v };
                case 5:
                    return new Color { A = 255, R = v, G = p, B = q };
                default:
                    return new Color { A = 255, R = v, G = t, B = p };
            }
        }
    }
}
