using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.ViewModel
{
    public abstract class TransformEffectViewModel : EffectViewModel
    {
        public TransformEffectViewModel(TransformEffect transformEffect) : base(transformEffect) { }

        public TransformEffect TransformEffect => (TransformEffect)Effect;

        public double From
        {
            get => TransformEffect.From;
            set
            {
                TransformEffect.From = value;
                RaisePropertyChanged(nameof(From));
            }
        }

        public double To
        {
            get => TransformEffect.To;
            set
            {
                TransformEffect.To = value;
                RaisePropertyChanged(nameof(To));
            }
        }

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
