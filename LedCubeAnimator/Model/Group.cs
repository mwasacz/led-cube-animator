using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.Model
{
    class Group : Effect
    {
        public ColorBlendMode ColorBlendMode { get; set; }
    }

    enum ColorBlendMode { Add, Multiply };
}
