using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model
{
    public abstract class Effect : Tile
    {
        public bool Reverse { get; set; }
        public int RepeatCount { get; set; } = 1;
        public TimeInterpolation TimeInterpolation { get; set; }
        public bool PersistEffect { get; set; }

        protected double Fraction(double time)
        {
            time = Math.Min(Math.Max(Start, time), End);
            if (time == End)
            {
                return Reverse ? 0 : 1;
            }

            double frac = RepeatCount * (time - Start) / (End - Start) % 1;
            
            if (Reverse)
            {
                frac = 1 - Math.Abs(frac * 2 - 1);
            }

            switch (TimeInterpolation)
            {
                case TimeInterpolation.Accelerate:
                    frac *= frac;
                    break;
                case TimeInterpolation.Decelerate:
                    frac = Math.Sqrt(frac);
                    break;
                case TimeInterpolation.Sine:
                    frac = (1 - Math.Cos(frac * Math.PI)) / 2;
                    break;
            }

            return frac;
        }
    }
}
