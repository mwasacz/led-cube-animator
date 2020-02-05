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
    [CategoryOrder("ShearEffect", 3)]
    public class ShearEffectViewModel : TransformEffectViewModel
    {
        public ShearEffectViewModel(ShearEffect shearEffect) : base(shearEffect) { }

        public ShearEffect ShearEffect => (ShearEffect)TransformEffect;

        [Category("ShearEffect")]
        [PropertyOrder(0)]
        public Plane Plane
        {
            get => ShearEffect.Plane;
            set
            {
                ShearEffect.Plane = value;
                RaisePropertyChanged(nameof(Plane));
            }
        }

        [Category("ShearEffect")]
        [PropertyOrder(1)]
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
