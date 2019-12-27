using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.ViewModel
{
    public class ScaleEffectViewModel : TransformEffectViewModel
    {
        public ScaleEffectViewModel(ScaleEffect scaleEffect) : base(scaleEffect) { }

        public ScaleEffect ScaleEffect => (ScaleEffect)TransformEffect;

        public Axis Axis
        {
            get => ScaleEffect.Axis;
            set
            {
                ScaleEffect.Axis = value;
                RaisePropertyChanged(nameof(Axis));
            }
        }

        public double Center
        {
            get => ScaleEffect.Center;
            set
            {
                ScaleEffect.Center = value;
                RaisePropertyChanged(nameof(Center));
            }
        }
    }
}
