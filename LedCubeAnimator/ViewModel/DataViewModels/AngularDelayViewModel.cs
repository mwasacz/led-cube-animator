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
        public AngularDelayViewModel(AngularDelay angularDelay, IModelManager model) : base(angularDelay, model) { }

        public AngularDelay AngularDelay => (AngularDelay)Delay;

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
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.Center), value);
        }

        [Category("AngularDelay")]
        [PropertyOrder(2)]
        public double StartAngle
        {
            get => AngularDelay.StartAngle;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.StartAngle), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(AngularDelay.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(AngularDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
                case nameof(AngularDelay.StartAngle):
                    RaisePropertyChanged(nameof(StartAngle));
                    break;
            }
        }
    }
}
