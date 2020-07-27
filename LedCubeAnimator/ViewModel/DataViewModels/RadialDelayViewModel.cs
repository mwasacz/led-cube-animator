using System.ComponentModel;
using System.Windows;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("RadialDelay", 2)]
    public class RadialDelayViewModel : DelayViewModel
    {
        public RadialDelayViewModel(RadialDelay radialDelay, IModelManager model) : base(radialDelay, model) { }

        public RadialDelay RadialDelay => (RadialDelay)Delay;

        [Category("RadialDelay")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => RadialDelay.Axis;
            set => Model.SetTileProperty(RadialDelay, nameof(RadialDelay.Axis), value);
        }

        [Category("RadialDelay")]
        [PropertyOrder(1)]
        public Point Center
        {
            get => RadialDelay.Center;
            set => Model.SetTileProperty(RadialDelay, nameof(RadialDelay.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(RadialDelay.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(RadialDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
