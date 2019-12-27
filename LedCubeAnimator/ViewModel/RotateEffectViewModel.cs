using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.ViewModel
{
    public class RotateEffectViewModel : TransformEffectViewModel
    {
        public RotateEffectViewModel(RotateEffect rotateEffect) : base(rotateEffect) { }

        public RotateEffect RotateEffect => (RotateEffect)TransformEffect;

        public Axis Axis
        {
            get => RotateEffect.Axis;
            set
            {
                RotateEffect.Axis = value;
                RaisePropertyChanged(nameof(Axis));
            }
        }

        public Point Center
        {
            get => RotateEffect.Center;
            set
            {
                RotateEffect.Center = value;
                RaisePropertyChanged(nameof(Center));
            }
        }
    }
}
