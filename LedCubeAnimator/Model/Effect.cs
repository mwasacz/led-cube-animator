using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model
{
    public abstract class Effect : Tile
    {
        public Direction Direction { get; set; }
        public int RepeatCount { get; set; }
        public TimeInterpolation TimeInterpolation { get; set; }
        public bool PersistEffect { get; set; }
    }

    public enum Direction { Fwd, Bwd, FwdBwd, BwdFwd };
    public enum TimeInterpolation { Linear, Accelerate, Decelerate, Sine };
}
