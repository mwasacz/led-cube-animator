using Jace;
using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.Model.Animations.Data
{
    public class CustomTile : Tile
    {
        public CustomTile()
        {
            var options = new JaceOptions { CultureInfo = CultureInfo.InvariantCulture, CacheMaximumSize = 1, CacheReductionSize = 1 };
            _engine = new CalculationEngine(options);
            _engine.AddFunction("r", x => IntToColor((int)x).R);
            _engine.AddFunction("g", x => IntToColor((int)x).G);
            _engine.AddFunction("b", x => IntToColor((int)x).B);
            _engine.AddFunction("color", (r, g, b) => ColorToInt((int)r, (int)g, (int)b));
            _engine.AddFunction("rand", x => new Random((int)x).Next());
        }

        private readonly CalculationEngine _engine;
        private string _expression = "parent(x, y, z, time)";
        private bool _valid = true;

        public string Expression
        {
            get => _expression;
            set
            {
                if (_expression != value)
                {
                    _expression = value;
                    _valid = true;
                }
            }
        }

        public override Color GetVoxel(Point3D point, double time, Func<Point3D, double, Color> getVoxel)
        {
            double result = 0;

            if (_valid)
            {
                _engine.AddConstant("x", point.X);
                _engine.AddConstant("y", point.Y);
                _engine.AddConstant("z", point.Z);
                _engine.AddConstant("time", time);
                _engine.AddConstant("start", Start);
                _engine.AddConstant("end", End);
                _engine.AddConstant("length", GetLength());
                _engine.AddFunction("parent", (x, y, z, t) => ColorToInt(getVoxel(new Point3D(x, y, z), t)));

                try
                {
                    result = _engine.Calculate(_expression);
                }
                catch
                {
                    _valid = false;
                }
            }

            return IntToColor((int)result);
        }

        private static int ColorToInt(Color color) => ColorToInt(color.R, color.G, color.B);

        private static int ColorToInt(int r, int g, int b) => (r << 16) + (g << 8) + b;

        private static Color IntToColor(int x) => Color.FromRgb((byte)(x >> 16), (byte)(x >> 8), (byte)x);
    }
}
