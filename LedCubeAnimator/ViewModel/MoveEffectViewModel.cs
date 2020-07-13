using GalaSoft.MvvmLight;
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
    [CategoryOrder("MoveEffect", 3)]
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect, IModelManager model) : base(moveEffect, model) { }

        public MoveEffect MoveEffect => (MoveEffect)TransformEffect;

        [Category("MoveEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => MoveEffect.Axis;
            set => Model.SetTileProperty(MoveEffect, nameof(MoveEffect.Axis), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(MoveEffect.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
            }
        }
    }
}
