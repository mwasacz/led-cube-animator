using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("RotateEffect", 3)]
    public class RotateEffectViewModel : TransformEffectViewModel
    {
        public RotateEffectViewModel(RotateEffect rotateEffect) : base(rotateEffect) { }

        public RotateEffect RotateEffect => (RotateEffect)TransformEffect;

        [Category("RotateEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => RotateEffect.Axis;
            set
            {
                RotateEffect.Axis = value;
                RaisePropertyChanged(nameof(Axis));
            }
        }

        [Category("RotateEffect")]
        [PropertyOrder(1)]
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
