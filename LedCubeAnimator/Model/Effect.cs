using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model
{
    class Effect : Tile
    {
        public Direction Direction { get; set; }
        public int RepeatCount { get; set; }
        public TimeInterpolation TimeInterpolation { get; set; }
        public bool PersistEffect { get; set; }
    }

    enum Direction { Fwd, Bwd, FwdBwd, BwdFwd };
    enum TimeInterpolation { Linear, Accelerate, Decelerate, Sine };
}
