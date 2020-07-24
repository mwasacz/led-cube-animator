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
        public SphericalDelayViewModel(SphericalDelay sphericalDelay, IModelManager model) : base(sphericalDelay, model) { }

        public SphericalDelay SphericalDelay => (SphericalDelay)Delay;

        [Category("SphericalDelay")]
        [PropertyOrder(0)]
        public Point3D Center
        {
            get => SphericalDelay.Center;
            set => Model.SetTileProperty(SphericalDelay, nameof(SphericalDelay.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SphericalDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
