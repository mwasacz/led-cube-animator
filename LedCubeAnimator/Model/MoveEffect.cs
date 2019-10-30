using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    class MoveEffect : Effect
    {
        public Vector3D From { get; set; }
        public Vector3D To { get; set; }
    }
}
