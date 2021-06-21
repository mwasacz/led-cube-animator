using System;
using System.Windows.Media;

namespace LedCubeAnimator.Utils
{
    public struct HsvColor : IEquatable<HsvColor>
    {
        public HsvColor(double h, double s, double v)
        {
            H = (h % 360 + 360) % 360;
            S = s;
            V = v;
        }

        public double H { get; }
        public double S { get; }
        public double V { get; }

        public Color ToRgb()
        {
            int i = (int)(H / 60);
            double f = H / 60 - i;

            byte v = (byte)(V + 0.5);
            byte p = (byte)(V * (255 - S) / 255 + 0.5);
            byte q = (byte)(V * (255 - S * f) / 255 + 0.5);
            byte t = (byte)(V * (255 - S * (1 - f)) / 255 + 0.5);

            switch (i)
            {
                case 0:
                    return Color.FromRgb(v, t, p);
                case 1:
                    return Color.FromRgb(q, v, p);
                case 2:
                    return Color.FromRgb(p, v, t);
                case 3:
                    return Color.FromRgb(p, q, v);
                case 4:
                    return Color.FromRgb(t, p, v);
                default:
                    return Color.FromRgb(v, p, q);
            }
        }

        public static HsvColor FromRgb(Color color)
        {
            double r = color.R;
            double g = color.G;
            double b = color.B;

            double max = Math.Max(Math.Max(r, g), b);
            double min = Math.Min(Math.Min(r, g), b);
            double delta = max - min;

            double h;
            double s = max == 0 ? 0 : delta * 255 / max;
            double v = max;

            if (delta == 0)
            {
                h = 0;
            }
            else if (r == max)
            {
                h = ((g - b) / delta + 6) % 6 * 60;
            }
            else if (g == max)
            {
                h = ((b - r) / delta + 2) * 60;
            }
            else
            {
                h = ((r - g) / delta + 4) * 60;
            }

            return new HsvColor(h, s, v);
        }

        public override bool Equals(object obj)
        {
            return obj is HsvColor color && Equals(color);
        }

        public bool Equals(HsvColor other)
        {
            return H == other.H && S == other.S && V == other.V;
        }

        public override int GetHashCode()
        {
            int hashCode = -1397884734;
            hashCode = hashCode * -1521134295 + H.GetHashCode();
            hashCode = hashCode * -1521134295 + S.GetHashCode();
            hashCode = hashCode * -1521134295 + V.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(HsvColor left, HsvColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HsvColor left, HsvColor right)
        {
            return !(left == right);
        }
    }
}
