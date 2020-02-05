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
    [CategoryOrder("ScaleEffect", 3)]
    public class ScaleEffectViewModel : TransformEffectViewModel
    {
        public ScaleEffectViewModel(ScaleEffect scaleEffect) : base(scaleEffect) { }

        public ScaleEffect ScaleEffect => (ScaleEffect)TransformEffect;

        [Category("ScaleEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => ScaleEffect.Axis;
            set
            {
                ScaleEffect.Axis = value;
                RaisePropertyChanged(nameof(Axis));
            }
        }

        [Category("ScaleEffect")]
        [PropertyOrder(1)]
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
