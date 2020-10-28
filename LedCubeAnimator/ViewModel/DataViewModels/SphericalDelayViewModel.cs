using System.ComponentModel;
using System.Windows.Media.Media3D;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("SphericalDelay", 2)]
    public class SphericalDelayViewModel : DelayViewModel
    {
        public SphericalDelayViewModel(SphericalDelay sphericalDelay, IModelManager model, GroupViewModel parent) : base(sphericalDelay, model, parent) { }

        [Browsable(false)]
        public SphericalDelay SphericalDelay => (SphericalDelay)Tile;

        [Category("SphericalDelay")]
        [PropertyOrder(0)]
        public Point3D Center
        {
            get => SphericalDelay.Center;
            set => Model.SetTileProperty(SphericalDelay, nameof(SphericalDelay.Center), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == SphericalDelay)
            {
                switch (propertyName)
                {
                    case nameof(SphericalDelay.Center):
                        changedProperty = nameof(Center);
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
