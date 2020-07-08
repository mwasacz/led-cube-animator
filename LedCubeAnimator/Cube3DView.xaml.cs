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
using HelixToolkit.Wpf;

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
        }



        public Color[,,] Frame
        {
            get => (Color[,,])GetValue(FrameProperty);
            set => SetValue(FrameProperty, value);
        }

        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register("Frame", typeof(Color[,,]), typeof(Cube3DView), new PropertyMetadata(new PropertyChangedCallback(OnFrameChanged)));

        private static void OnFrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((Cube3DView)d).OnFrameChanged(e);

        private void OnFrameChanged(DependencyPropertyChangedEventArgs e) => UpdateSpheres(e.OldValue as Color[,,], e.NewValue as Color[,,]);



        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(Cube3DView));



        private GeometryModel3D[,,] _spheres;

        private Dictionary<Color, Material> _materials = new Dictionary<Color, Material>();



        private void UpdateSpheres(Color[,,] oldFrame, Color[,,] newFrame)
        {
            var oldVoxels = oldFrame ?? new Color[0, 0, 0];
            var newVoxels = newFrame ?? new Color[0, 0, 0];

            if (oldVoxels.GetLength(0) != newVoxels.GetLength(0)
                || oldVoxels.GetLength(1) != newVoxels.GetLength(1)
                || oldVoxels.GetLength(2) != newVoxels.GetLength(2))
            {
                CreateSpheres(newVoxels);
            }

            UpdateMaterials(newVoxels);

            for (int x = 0; x < newVoxels.GetLength(0); x++)
            {
                for (int y = 0; y < newVoxels.GetLength(1); y++)
                {
                    for (int z = 0; z < newVoxels.GetLength(2); z++)
                    {
                        var material = _materials[newVoxels[x, y, z]];
                        _spheres[x, y, z].Material = material;
                        //_spheres[x, y, z].BackMaterial = material;
                    }
                }
            }
        }

        private void CreateSpheres(Color[,,] newVoxels)
        {
            SortingVisual3D.Children.Clear();

            _spheres = new GeometryModel3D[newVoxels.GetLength(0), newVoxels.GetLength(1), newVoxels.GetLength(2)];

            var mesh = CreateSphereMesh(newVoxels.GetLength(0) * newVoxels.GetLength(1) * newVoxels.GetLength(2));

            Point3D offset = new Point3D(
                0.5 * (newVoxels.GetLength(0) - 1),
                0.5 * (newVoxels.GetLength(1) - 1),
                0.5 * (newVoxels.GetLength(2) - 1));

            for (int x = 0; x < newVoxels.GetLength(0); x++)
            {
                for (int y = 0; y < newVoxels.GetLength(1); y++)
                {
                    for (int z = 0; z < newVoxels.GetLength(2); z++)
                    {
                        var model = new GeometryModel3D
                        {
                            Geometry = mesh,
                            Transform = new TranslateTransform3D(new Point3D(x, y, z) - offset)
                        };

                        _spheres[x, y, z] = model;

                        SortingVisual3D.Children.Add(new ModelVisual3D { Content = model });
                    }
                }
            }
        }

        private static MeshGeometry3D CreateSphereMesh(int sphereCount)
        {
            int div = (int)Math.Sqrt((double)65536 / sphereCount);

            //var builder = new MeshBuilder(false, false);
            //builder.AddSphere(new Point3D(0, 0, 0), 0.25, 2 * div, div);
            //return builder.ToMesh();

            return CreateSphereMesh(new Point3D(0, 0, 0), 0.25, div);
        }

        private static MeshGeometry3D CreateSphereMesh(Point3D center, double radius, int div)
        {
            double d = Math.PI / div;

            Point3DCollection positions = new Point3DCollection();

            positions.Add(new Point3D(center.X, center.Y, center.Z + radius));

            for (int i = 1; i < div; i++)
            {
                double phi = i * d;

                for (int j = 0; j < 2 * div; j++)
                {
                    double theta = j * d;

                    double x = Math.Cos(theta) * Math.Sin(phi);
                    double y = Math.Sin(theta) * Math.Sin(phi);
                    double z = Math.Cos(phi);

                    positions.Add(new Point3D(center.X + radius * x, center.Y + radius * y, center.Z + radius * z));
                }
            }

            positions.Add(new Point3D(center.X, center.Y, center.Z - radius));

            Int32Collection triangleIndices = new Int32Collection();

            int last = (div - 1) * 2 * div + 1;

            for (int j = 0; j < 2 * div; j++)
            {
                int next = j == 2 * div - 1 ? 1 - 2 * div : 1;

                triangleIndices.Add(j + 1 + next);
                triangleIndices.Add(0);
                triangleIndices.Add(j + 1);

                for (int i = 0; i < div - 2; i++)
                {
                    int ij = i * 2 * div + j + 1;

                    triangleIndices.Add(ij);
                    triangleIndices.Add(ij + 2 * div + next);
                    triangleIndices.Add(ij + next);

                    triangleIndices.Add(ij + 2 * div + next);
                    triangleIndices.Add(ij);
                    triangleIndices.Add(ij + 2 * div);
                }

                triangleIndices.Add(last - 2 * div + j);
                triangleIndices.Add(last);
                triangleIndices.Add(last - 2 * div + j + next);
            }

            return new MeshGeometry3D
            {
                Positions = positions,
                TriangleIndices = triangleIndices
            };
        }

        private void UpdateMaterials(Color[,,] newVoxels)
        {
            var newMaterials = new Dictionary<Color, Material>();

            foreach (Color voxel in newVoxels)
            {
                if (!newMaterials.ContainsKey(voxel))
                {
                    if (!_materials.TryGetValue(voxel, out var material))
                    {
                        var sphereColor = new Color
                        {
                            A = (byte)(Math.Max(Math.Max(voxel.R, voxel.G), voxel.B) * 7 / 8 + 32),
                            R = voxel.R,
                            G = voxel.G,
                            B = voxel.B
                        };
                        material = new DiffuseMaterial(new SolidColorBrush(sphereColor));
                    }
                    newMaterials.Add(voxel, material);
                }
            }

            _materials = newMaterials;
        }



        private void MainViewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                HitTest(e.GetPosition(MainViewport));
            }
        }

        public void HitTest(Point mousePos)
        {
            var model = MainViewport.Viewport.FindHits(mousePos).FirstOrDefault()?.Model;

            if (model != null)
            {
                for (int x = 0; x < _spheres.GetLength(0); x++)
                {
                    for (int y = 0; y < _spheres.GetLength(1); y++)
                    {
                        for (int z = 0; z < _spheres.GetLength(2); z++)
                        {
                            if (_spheres[x, y, z] == model)
                            {
                                Command?.Execute(new Point3D(x, y, z));
                            }
                        }
                    }
                }
            }
        }
    }
}
