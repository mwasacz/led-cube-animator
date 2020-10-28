using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Frame", 1)]
    public class FrameViewModel : TileViewModel
    {
        public FrameViewModel(Frame frame, IModelManager model, GroupViewModel parent) : base(frame, model, parent) { }

        [Browsable(false)]
        public Frame Frame => (Frame)Tile;

        [Category("Frame")]
        [PropertyOrder(0)]
        public object Voxels { get; }

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

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == Frame)
            {
                switch (propertyName)
                {
                    case nameof(Frame.Offset):
                        changedProperty = nameof(Offset);
                        break;
                    case nameof(Frame.Voxels):
                        RaisePropertyChanged(nameof(Size)); // ToDo
                        changedProperty = nameof(Voxels);
                        break;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
            }
        }
    }
}
