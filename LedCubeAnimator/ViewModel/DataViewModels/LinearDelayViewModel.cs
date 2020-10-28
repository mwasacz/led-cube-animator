using System.ComponentModel;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("LinearDelay", 2)]
    public class LinearDelayViewModel : DelayViewModel
    {
        public LinearDelayViewModel(LinearDelay linearDelay, IModelManager model, GroupViewModel parent) : base(linearDelay, model, parent) { }

        [Browsable(false)]
        public LinearDelay LinearDelay => (LinearDelay)Tile;

        [Category("LinearDelay")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => LinearDelay.Axis;
            set => Model.SetTileProperty(LinearDelay, nameof(LinearDelay.Axis), value);
        }

        [Category("LinearDelay")]
        [PropertyOrder(1)]
        public double Center
        {
            get => LinearDelay.Center;
            set => Model.SetTileProperty(LinearDelay, nameof(LinearDelay.Center), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == LinearDelay)
            {
                switch (propertyName)
                {
                    case nameof(LinearDelay.Axis):
                        changedProperty = nameof(Axis);
                        break;
                    case nameof(LinearDelay.Center):
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
