using System.ComponentModel;
using System.Windows;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("AngularDelay", 2)]
    public class AngularDelayViewModel : DelayViewModel
    {
        public AngularDelayViewModel(AngularDelay angularDelay, IModelManager model, GroupViewModel parent) : base(angularDelay, model, parent) { }

        [Browsable(false)]
        public AngularDelay AngularDelay => (AngularDelay)Tile;

        [Category("AngularDelay")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => AngularDelay.Axis;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.Axis), value);
        }

        [Category("AngularDelay")]
        [PropertyOrder(1)]
        public Point Center
        {
            get => AngularDelay.Center;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.Center), GetNewValue(value, AngularDelay.Center));
        }

        [Category("AngularDelay")]
        [PropertyOrder(2)]
        public double StartAngle
        {
            get => AngularDelay.StartAngle;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.StartAngle), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == AngularDelay)
            {
                switch (propertyName)
                {
                    case nameof(AngularDelay.Axis):
                        changedProperty = nameof(Axis);
                        break;
                    case nameof(AngularDelay.Center):
                        changedProperty = nameof(Center);
                        break;
                    case nameof(AngularDelay.StartAngle):
                        changedProperty = nameof(StartAngle);
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
