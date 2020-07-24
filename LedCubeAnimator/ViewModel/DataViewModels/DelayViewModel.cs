using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Model.Undo;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Delay", 1)]
    public class DelayViewModel : TileViewModel
    {
        public DelayViewModel(Delay delay, IModelManager model) : base(delay, model) { }

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

        public override void ModelPropertyChanged(string propertyName)
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
            }
        }
    }
}
