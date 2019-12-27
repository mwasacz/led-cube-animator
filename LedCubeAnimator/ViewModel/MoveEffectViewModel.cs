using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.ViewModel
{
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect) : base(moveEffect) { }

        public MoveEffect MoveEffect => (MoveEffect)TransformEffect;

        public Axis Axis
        {
            get => MoveEffect.Axis;
            set
            {
                MoveEffect.Axis = value;
                RaisePropertyChanged(nameof(Axis));
            }
        }
    }
}
