using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("LinearDelay", 2)]
    public class LinearDelayViewModel : DelayViewModel
    {
        public LinearDelayViewModel(LinearDelay linearDelay, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(linearDelay, model, messenger, parent) { }

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

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(LinearDelay.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(LinearDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
