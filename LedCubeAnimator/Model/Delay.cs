﻿using System;
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
                var length = GetLength();
                time = ((time - Start) % length + length) % length + Start;
            }

            return time;
        }
    }
}
