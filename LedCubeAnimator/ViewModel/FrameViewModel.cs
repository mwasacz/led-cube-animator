using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.ViewModel
{
    public class FrameViewModel : TileViewModel
    {
        public FrameViewModel(Frame frame) : base(frame)
        {
            UpdateVoxels();

            Voxels.CollectionChanged += Voxels_CollectionChanged;
        }

        public Frame Frame => (Frame)Tile;

        public Point3D From
        {
            get => Frame.Offset;
            set
            {
                Frame.Offset = value;
                RaisePropertyChanged(nameof(From));
                RaisePropertyChanged(nameof(To));
            }
        }

        public Point3D To
        {
            get => Frame.Offset + new Vector3D(Frame.Voxels.GetLength(0) - 1, Frame.Voxels.GetLength(1) - 1, Frame.Voxels.GetLength(2) - 1);
            set
            {
                var oldVoxels = Frame.Voxels;
                int oldX = (int)To.X - (int)From.X + 1;
                int oldY = (int)To.Y - (int)From.Y + 1;
                int oldZ = (int)To.Z - (int)From.Z + 1;
                int newX = (int)value.X - (int)From.X + 1;
                int newY = (int)value.Y - (int)From.Y + 1;
                int newZ = (int)value.Z - (int)From.Z + 1;
                Frame.Voxels = new Color[newX, newY, newZ];
                for (int x = 0; x < newX; x++)
                {
                    for (int y = 0; y < newY; y++)
                    {
                        for (int z = 0; z < newZ; z++)
                        {
                            Frame.Voxels[x, y, z] = x < oldX && y < oldY && z < oldZ ? oldVoxels[x, y, z] : Colors.Black;
                        }
                    }
                }

                Voxels.CollectionChanged -= Voxels_CollectionChanged;
                UpdateVoxels();
                Voxels.CollectionChanged += Voxels_CollectionChanged;
            }
        }

        public ObservableCollection<Color> Voxels { get; } = new ObservableCollection<Color>();

        private void Voxels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int i = 0;
            int maxX = (int)To.X - (int)From.X + 1;
            int maxY = (int)To.Y - (int)From.Y + 1;
            int maxZ = (int)To.Z - (int)From.Z + 1;
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    for (int z = 0; z < maxZ; z++)
                    {
                        Frame.Voxels[x, y, z] = Voxels[i];
                        i++;
                    }
                }
            }
        }

        private void UpdateVoxels()
        {
            Voxels.Clear();
            int maxX = (int)To.X - (int)From.X + 1;
            int maxY = (int)To.Y - (int)From.Y + 1;
            int maxZ = (int)To.Z - (int)From.Z + 1;
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    for (int z = 0; z < maxZ; z++)
                    {
                        Voxels.Add(Frame.Voxels[x, y, z]);
                    }
                }
            }
        }
    }
}
