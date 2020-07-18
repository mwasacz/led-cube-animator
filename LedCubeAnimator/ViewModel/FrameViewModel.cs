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
        public FrameViewModel(Frame frame, IModelManager model) : base(frame, model) { }

        public Frame Frame => (Frame)Tile;

        [Category("Frame")]
        [PropertyOrder(1)]
        public Point3D Offset
        {
            get => Frame.Offset;
            set => Model.SetTileProperty(Frame, nameof(Frame.Offset), value);
        }

        [Category("Frame")]
        [PropertyOrder(2)]
        public Vector3D Size
        {
            get => new Vector3D(Frame.Voxels.GetLength(0), Frame.Voxels.GetLength(1), Frame.Voxels.GetLength(2));
            set => Model.SetTileProperty(Frame, nameof(Frame.Voxels), new Color[(int)value.X, (int)value.Y, (int)value.Z]);
        }

        [Category("Frame")]
        [DisplayName("Voxels")]
        [PropertyOrder(0)]
        public object VoxelsProperty { get; }

        public Color[,,] Voxels => Frame.Voxels;

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Frame.Offset):
                    RaisePropertyChanged(nameof(Offset));
                    break;
                case nameof(Frame.Voxels):
                    RaisePropertyChanged(nameof(Size)); // ToDo
                    RaisePropertyChanged(nameof(Voxels));
                    break;
            }
        }
    }
}
