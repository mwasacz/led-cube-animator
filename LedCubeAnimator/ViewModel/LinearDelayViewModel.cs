using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("LinearDelayEffect", 2)]
    public class LinearDelayViewModel : DelayViewModel
    {
        public LinearDelayViewModel(LinearDelay linearDelay, IModelManager model) : base(linearDelay, model) { }

        public LinearDelay LinearDelayEffect => (LinearDelay)Delay;

        [Category("LinearDelayEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => LinearDelayEffect.Axis;
            set => Model.SetTileProperty(LinearDelayEffect, nameof(LinearDelayEffect.Axis), value);
        }

        [Category("LinearDelayEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => LinearDelayEffect.Center;
            set => Model.SetTileProperty(LinearDelayEffect, nameof(LinearDelayEffect.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
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
