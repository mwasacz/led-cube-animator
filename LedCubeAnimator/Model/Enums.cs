using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model
{
    public enum ColorMode { Mono, MonoBrightness, RGB }

    public enum TimeInterpolation { Linear, Accelerate, Decelerate, Sine }

    public enum ColorBlendMode { Add, Multiply, Min, Max, Average }

    public enum Axis { X, Y, Z }

    public enum Plane { XY, XZ, YX, YZ, ZX, ZY }
}