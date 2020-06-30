using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
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
        public ScaleEffectViewModel(ScaleEffect scaleEffect, UndoManager undo) : base(scaleEffect, undo) { }

        public ScaleEffect ScaleEffect => (ScaleEffect)TransformEffect;

        [Category("ScaleEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => ScaleEffect.Axis;
            set => Undo.Set(ScaleEffect, nameof(ScaleEffect.Axis), value);
        }

        [Category("ScaleEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => ScaleEffect.Center;
            set => Undo.Set(ScaleEffect, nameof(ScaleEffect.Center), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(ScaleEffect.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(ScaleEffect.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
