using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.Frame))]
    public class FrameViewModel : TileViewModel
    {
        public FrameViewModel(Frame frame, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(frame, model, messenger, parent) { }

        [Browsable(false)]
        public Frame Frame => (Frame)Tile;

        [Category("Frame")]
        [PropertyOrder(10)]
        public string Voxels => "Edit in 3D View";

        [Category("Frame")]
        [PropertyOrder(11)]
        public Point3D Offset
        {
            get => Frame.Offset;
            set => Model.SetTileProperty(Frame, nameof(Frame.Offset), GetNewValue(value, Frame.Offset));
        }

        [Category("Frame")]
        [PropertyOrder(12)]
        public Vector3D Size
        {
            get => new Vector3D(Frame.Voxels.GetLength(0), Frame.Voxels.GetLength(1), Frame.Voxels.GetLength(2));
            set
            {
                var size = GetNewValue(value, Size);
                Model.SetTileProperty(Frame, nameof(Frame.Voxels), new Color[(int)size.X, (int)size.Y, (int)size.Z]);
            }
        }

        protected override void ModelPropertyChanged(string propertyName)
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
