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
                int len = End - Start;
                time = ((time - Start) % len + len) % len + Start;
            }

            return time;
        }
    }
}
