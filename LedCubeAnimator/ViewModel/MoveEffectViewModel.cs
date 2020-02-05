using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("MoveEffect", 3)]
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect) : base(moveEffect) { }

        public MoveEffect MoveEffect => (MoveEffect)TransformEffect;

        [Category("MoveEffect")]
        [PropertyOrder(0)]
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
