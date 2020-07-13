using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
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
        public RotateEffectViewModel(RotateEffect rotateEffect, IModelManager model) : base(rotateEffect, model) { }

        public RotateEffect RotateEffect => (RotateEffect)TransformEffect;

        [Category("RotateEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => RotateEffect.Axis;
            set => Model.SetTileProperty(RotateEffect, nameof(RotateEffect.Axis), value);
        }

        [Category("RotateEffect")]
        [PropertyOrder(1)]
        public Point Center
        {
            get => RotateEffect.Center;
            set => Model.SetTileProperty(RotateEffect, nameof(RotateEffect.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(RotateEffect.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(RotateEffect.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
