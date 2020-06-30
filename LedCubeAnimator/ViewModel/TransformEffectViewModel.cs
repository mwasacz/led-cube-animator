using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("TransformEffect", 2)]
    public abstract class TransformEffectViewModel : EffectViewModel
    {
        public TransformEffectViewModel(TransformEffect transformEffect, UndoManager undo) : base(transformEffect, undo) { }

        public TransformEffect TransformEffect => (TransformEffect)Effect;

        [Category("TransformEffect")]
        [PropertyOrder(0)]
        public double From
        {
            get => TransformEffect.From;
            set => Undo.Set(TransformEffect, nameof(TransformEffect.From), value);
        }

        [Category("TransformEffect")]
        [PropertyOrder(1)]
        public double To
        {
            get => TransformEffect.To;
            set => Undo.Set(TransformEffect, nameof(TransformEffect.To), value);
        }

        [Category("TransformEffect")]
        [PropertyOrder(2)]
        public bool Round
        {
            get => TransformEffect.Round;
            set => Undo.Set(TransformEffect, nameof(TransformEffect.Round), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(TransformEffect.From):
                    RaisePropertyChanged(nameof(From));
                    break;
                case nameof(TransformEffect.To):
                    RaisePropertyChanged(nameof(To));
                    break;
                case nameof(TransformEffect.Round):
                    RaisePropertyChanged(nameof(Round));
                    break;
            }
        }
    }
}
