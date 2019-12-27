using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.ViewModel
{
    public class ShearEffectViewModel : TransformEffectViewModel
    {
        public ShearEffectViewModel(ShearEffect shearEffect) : base(shearEffect) { }

        public ShearEffect ShearEffect => (ShearEffect)TransformEffect;

        public Plane Plane
        {
            get => ShearEffect.Plane;
            set
            {
                ShearEffect.Plane = value;
                RaisePropertyChanged(nameof(Plane));
            }
        }

        public double Center
        {
            get => ShearEffect.Center;
            set
            {
                ShearEffect.Center = value;
                RaisePropertyChanged(nameof(Center));
            }
        }
    }
}
