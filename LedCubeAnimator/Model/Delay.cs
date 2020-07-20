using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model
{
    public abstract class Delay : Tile
    {
        public double Value { get; set; }

        public bool WrapAround { get; set; }

        protected double GetDelayedTime(double time, double distance)
        {
            time -= distance * Value;

            if (WrapAround)
            {
                double t = (time - Start) % (End - Start);
                time = t < 0 ? t + End : t + Start;
            }

            return time;
        }
    }
}
