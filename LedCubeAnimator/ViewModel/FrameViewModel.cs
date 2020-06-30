using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("Frame", 1)]
    public class FrameViewModel : TileViewModel
    {
        public FrameViewModel(Frame frame, UndoManager undo) : base(frame, undo) { }

        public Frame Frame => (Frame)Tile;

        [Category("Frame")]
        [PropertyOrder(1)]
        public Point3D From
        {
            get => Frame.Offset;
            set => Set(Frame, nameof(Frame.Offset), value);
        }

        [Category("Frame")]
        [PropertyOrder(2)]
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
                var voxels = new Color[newX, newY, newZ];
                for (int x = 0; x < newX; x++)
                {
                    for (int y = 0; y < newY; y++)
                    {
                        for (int z = 0; z < newZ; z++)
                        {
                            voxels[x, y, z] = x < oldX && y < oldY && z < oldZ ? oldVoxels[x, y, z] : Colors.Black;
                        }
                    }
                }
                Set(Frame, nameof(Frame.Voxels), voxels);
            }
        }

        [Category("Frame")]
        [PropertyOrder(0)]
        public Color[,,] Voxels { get; }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Frame.Offset):
                    RaisePropertyChanged(nameof(From));
                    RaisePropertyChanged(nameof(To));
                    break;
                case nameof(Frame.Voxels):
                    RaisePropertyChanged(nameof(To));
                    break;
            }
        }
    }
}
