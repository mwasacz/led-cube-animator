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
        public RadialDelayViewModel(RadialDelay radialDelay, IModelManager model, GroupViewModel parent) : base(radialDelay, model, parent) { }

        [Browsable(false)]
        public RadialDelay RadialDelay => (RadialDelay)Tile;

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
            set => Model.SetTileProperty(RadialDelay, nameof(RadialDelay.Center), GetNewValue(value, RadialDelay.Center));
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == RadialDelay)
            {
                switch (propertyName)
                {
                    case nameof(RadialDelay.Axis):
                        changedProperty = nameof(Axis);
                        break;
                    case nameof(RadialDelay.Center):
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
