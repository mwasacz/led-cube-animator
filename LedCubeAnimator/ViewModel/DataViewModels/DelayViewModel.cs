using System.ComponentModel;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Delay", 1)]
    public class DelayViewModel : TileViewModel
    {
        public DelayViewModel(Delay delay, IModelManager model, GroupViewModel parent) : base(delay, model, parent) { }

        [Browsable(false)]
        public Delay Delay => (Delay)Tile;

        [Category("Delay")]
        [PropertyOrder(0)]
        public double Value
        {
            get => Delay.Value;
            set => Model.SetTileProperty(Delay, nameof(Delay.Value), value);
        }

        [Category("Delay")]
        [PropertyOrder(1)]
        public bool WrapAround
        {
            get => Delay.WrapAround;
            set => Model.SetTileProperty(Delay, nameof(Delay.WrapAround), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == Delay)
            {
                switch (propertyName)
                {
                    case nameof(Delay.Value):
                        changedProperty = nameof(Value);
                        break;
                    case nameof(Delay.WrapAround):
                        changedProperty = nameof(WrapAround);
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
