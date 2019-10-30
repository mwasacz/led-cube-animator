using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    class RotateEffect : Effect
    {
        public AxisAngleRotation3D From { get; set; }
        public AxisAngleRotation3D To { get; set; }
    }
}
