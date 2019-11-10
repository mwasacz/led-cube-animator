using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator
{
    /// <summary>
    /// Interaction logic for Cube3DView.xaml
    /// </summary>
    public partial class Cube3DView : UserControl
    {
        public Cube3DView()
        {
            InitializeComponent();

            DefineCamera();
            DefineLights();
            //DefineMaterials();

            //CreateModels();
            //OrderModels();

            ModelVisual3D modelVisual = new ModelVisual3D();
            modelVisual.Content = _modelGroup;
            MainViewport.Children.Add(modelVisual);
        }



        public Color[,,] Frame
        {
            get => (Color[,,])GetValue(FrameProperty);
            set => SetValue(FrameProperty, value);
        }

        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register("Frame", typeof(Color[,,]), typeof(Cube3DView), new PropertyMetadata(new PropertyChangedCallback(OnFrameChanged)));

        private static void OnFrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Cube3DView)d).OnFrameChanged(e);

        private void OnFrameChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Frame == null)
            {
                return;
            }

            CreateModels(Frame);
            OrderModels();
        }



        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(Cube3DView));



        // 3D scene
        private Model3DGroup _modelGroup = new Model3DGroup();
        private PerspectiveCamera _camera;
        private Light[] _lights;

        // Transform applied to camera and lights
        private Transform3DGroup _transform = new Transform3DGroup();

        // List of spheres
        private Dictionary<GeometryModel3D, Point3D> _models = new Dictionary<GeometryModel3D, Point3D>();

        // Material for spheres
        private Dictionary<Color, Material> _materials;

        // Camera position
        private double _theta;
        private double _phi;
        private double _zoom = 400;

        // Mouse position
        private bool _mouseDown;
        private Point _mousePos;



        private void DefineCamera()
        {
            _camera = new PerspectiveCamera();
            _camera.FieldOfView = 60;
            _camera.Position = new Point3D(0, 0, _zoom / 100);

            _camera.Transform = _transform;

            MainViewport.Camera = _camera;
        }

        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light = new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));

            directional_light.Transform = _transform;

            _lights = new Light[] { ambient_light, directional_light };
        }

        /*private void DefineMaterials()
        {
            //MaterialGroup group = new MaterialGroup();
            //group.Children.Add(new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(0, 0, 0x80))));
            //group.Children.Add(new SpecularMaterial(new SolidColorBrush(Color.FromRgb(0, 0, 0x80)), 50));
            //group.Children.Add(new EmissiveMaterial(new SolidColorBrush(Color.FromRgb(0, 0, 0x64))));
            //_selectedMaterial = group;

            _selectedMaterial = new DiffuseMaterial(Brushes.Blue);
            _normalMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF)));
        }*/

        private MeshGeometry3D CreateSphere(double r, Point3D p, int n)
        {
            double segmentRad = Math.PI / n;
            int separators = 2 * n;

            Point3DCollection points = new Point3DCollection();
            for (int e = -n / 2; e <= n / 2; e++)
            {
                double r_e = r * Math.Cos(segmentRad * e);
                double y_e = r * Math.Sin(segmentRad * e);

                for (int s = 0; s <= separators - 1; s++)
                {
                    double z_s = r_e * Math.Sin(segmentRad * s) * -1;
                    double x_s = r_e * Math.Cos(segmentRad * s);
                    points.Add(new Point3D(p.X + x_s, p.Y + y_e, p.Z + z_s));
                }
            }
            points.Add(new Point3D(p.X, p.Y + r, p.Z));
            points.Add(new Point3D(p.X, p.Y - r, p.Z));

            Int32Collection triangleIndices = new Int32Collection();
            for (int i = 0; i < separators; i++)
            {
                triangleIndices.Add(i);
                triangleIndices.Add(separators * (n + 1) + 1);
                triangleIndices.Add((i + 1) % separators);

                for (int e = 0; e < n; e++)
                {
                    triangleIndices.Add(e * separators + i);
                    triangleIndices.Add(e * separators + (i + 1) % separators + separators);
                    triangleIndices.Add(e * separators + i + separators);

                    triangleIndices.Add(e * separators + (i + 1) % separators + separators);
                    triangleIndices.Add(e * separators + i);
                    triangleIndices.Add(e * separators + (i + 1) % separators);
                }

                triangleIndices.Add(n * separators + i);
                triangleIndices.Add(n * separators + (i + 1) % separators);
                triangleIndices.Add(separators * (n + 1));
            }

            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = points;
            mesh.TriangleIndices = triangleIndices;

            return mesh;
        }

        private void CreateModels(Color[,,] voxels)
        {
            var oldModels = _models.ToDictionary(m => m.Value, m => m.Key);

            _models = new Dictionary<GeometryModel3D, Point3D>();

            _materials = new Dictionary<Color, Material>();
            foreach (var voxel in voxels)
            {
                if (!_materials.ContainsKey(voxel))
                {
                    var sphereColor = voxel;
                    sphereColor.A = (byte)(Math.Min(voxel.R + voxel.G + voxel.B, 255) * 7 / 8 + 32);
                    _materials.Add(voxel, new DiffuseMaterial(new SolidColorBrush(sphereColor)));
                }
            }

            Vector3D center = new Vector3D(0.5 * (voxels.GetLength(0) - 1), 0.5 * (voxels.GetLength(1) - 1), 0.5 * (voxels.GetLength(2) - 1));

            int n = 1280 / voxels.Length;

            for (int x = 0; x < voxels.GetLength(0); x++)
            {
                for (int y = 0; y < voxels.GetLength(1); y++)
                {
                    for (int z = 0; z < voxels.GetLength(2); z++)
                    {
                        Point3D p = new Point3D(x, y, z);

                        if (oldModels.TryGetValue(p, out GeometryModel3D model))
                        {
                            model.Material = _materials[voxels[x, y, z]];
                        }
                        else
                        {
                            MeshGeometry3D mesh = CreateSphere(0.25, p - center, n);
                            model = new GeometryModel3D(mesh, _materials[voxels[x, y, z]]);
                        }

                        _models.Add(model, p);
                    }
                }
            }
        }

        private void OrderModels()
        {
            Point3D pos = _transform.Transform(_camera.Position);
            _modelGroup.Children = new Model3DCollection(
                _lights.AsEnumerable<Model3D>().Concat(
                _models.OrderByDescending(m => (pos - m.Value).Length).Select(m => m.Key)));
        }



        public void HitTest(Point mousePos)
        {
            HitTestResult result = VisualTreeHelper.HitTest(MainViewport, mousePos);

            if (result is RayMeshGeometry3DHitTestResult meshResult)
            {
                GeometryModel3D model = (GeometryModel3D)meshResult.ModelHit;

                Command?.Execute(_models[model]);
            }
        }



        public void Zoom()
        {
            // For zooming we simply change the Z-position of the camera
            _camera.Position = new Point3D(_camera.Position.X, _camera.Position.Y, _zoom / 100);
            OrderModels();
        }

        private void Rotate()
        {
            // Clamp phi (pitch) between -90 and 90 to avoid 'going upside down'
            if (_phi < -90) _phi = -90;
            if (_phi > 90) _phi = 90;

            Vector3D thetaAxis = new Vector3D(0, 1, 0);
            Vector3D phiAxis = new Vector3D(-1, 0, 0);

            _transform.Children.Clear();
            _transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(-phiAxis, _phi)));
            _transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(-thetaAxis, _theta)));

            // Z ordering
            OrderModels();
        }



        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _mouseDown = true;

                _mousePos = e.GetPosition(MainViewport);
            }
            else if (e.ChangedButton == MouseButton.Left)
            {
                HitTest(e.GetPosition(MainViewport));
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                Point newPos = e.GetPosition(MainViewport);

                _theta += (newPos.X - _mousePos.X) / 3;
                _phi += (_mousePos.Y - newPos.Y) / 3;

                _mousePos = newPos;

                Rotate();
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                _mouseDown = false;
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _zoom -= e.Delta;
            if (_zoom < 0)
            {
                _zoom = 0;
            }
            Zoom();
        }
    }
}
