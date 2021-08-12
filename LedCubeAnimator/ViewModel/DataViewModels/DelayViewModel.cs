using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Delay", 1)]
    public abstract class DelayViewModel : TileViewModel
    {
        public DelayViewModel(Delay delay, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(delay, model, messenger, parent) { }

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

        [Category("Delay")]
        [PropertyOrder(2)]
        public bool Static
        {
            get => Delay.Static;
            set => Model.SetTileProperty(Delay, nameof(Delay.Static), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Delay.Value):
                    RaisePropertyChanged(nameof(Value));
                    break;
                case nameof(Delay.WrapAround):
                    RaisePropertyChanged(nameof(WrapAround));
                    break;
                case nameof(Delay.Static):
                    RaisePropertyChanged(nameof(Static));
                    break;
            }
        }
    }
}
