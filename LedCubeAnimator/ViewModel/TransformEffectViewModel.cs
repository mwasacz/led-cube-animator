using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("TransformEffect", 2)]
    public abstract class TransformEffectViewModel : EffectViewModel
    {
        public TransformEffectViewModel(TransformEffect transformEffect) : base(transformEffect) { }

        public TransformEffect TransformEffect => (TransformEffect)Effect;

        [Category("TransformEffect")]
        [PropertyOrder(0)]
        public double From
        {
            get => TransformEffect.From;
            set
            {
                TransformEffect.From = value;
                RaisePropertyChanged(nameof(From));
            }
        }

        [Category("TransformEffect")]
        [PropertyOrder(1)]
        public double To
        {
            get => TransformEffect.To;
            set
            {
                TransformEffect.To = value;
                RaisePropertyChanged(nameof(To));
            }
        }

        [Category("TransformEffect")]
        [PropertyOrder(2)]
        public bool Round
        {
            get => TransformEffect.Round;
            set
            {
                TransformEffect.Round = value;
                RaisePropertyChanged(nameof(Round));
            }
        }
    }
}
