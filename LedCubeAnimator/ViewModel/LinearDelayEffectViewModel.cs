using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCubeAnimator.Model;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("LinearDelayEffect", 2)]
    public class LinearDelayEffectViewModel : EffectViewModel
    {
        public LinearDelayEffectViewModel(LinearDelayEffect linearDelayEffect) : base(linearDelayEffect) { }

        public LinearDelayEffect LinearDelayEffect => (LinearDelayEffect)Effect;

        [Category("LinearDelayEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => LinearDelayEffect.Axis;
            set
            {
                LinearDelayEffect.Axis = value;
                RaisePropertyChanged(nameof(Axis));
            }
        }

        [Category("LinearDelayEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => LinearDelayEffect.Center;
            set
            {
                LinearDelayEffect.Center = value;
                RaisePropertyChanged(nameof(Center));
            }
        }

        [Category("LinearDelayEffect")]
        [PropertyOrder(2)]
        public double Value
        {
            get => LinearDelayEffect.Value;
            set
            {
                LinearDelayEffect.Value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }
    }
}
