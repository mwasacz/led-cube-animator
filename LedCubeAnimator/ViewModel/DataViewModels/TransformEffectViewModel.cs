using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("TransformEffect", 2)]
    public abstract class TransformEffectViewModel : EffectViewModel
    {
        public TransformEffectViewModel(TransformEffect transformEffect, IModelManager model) : base(transformEffect, model) { }

        public TransformEffect TransformEffect => (TransformEffect)Effect;

        [Category("TransformEffect")]
        [PropertyOrder(0)]
        public double From
        {
            get => TransformEffect.From;
            set => Model.SetTileProperty(TransformEffect, nameof(TransformEffect.From), value);
        }

        [Category("TransformEffect")]
        [PropertyOrder(1)]
        public double To
        {
            get => TransformEffect.To;
            set => Model.SetTileProperty(TransformEffect, nameof(TransformEffect.To), value);
        }

        [Category("TransformEffect")]
        [PropertyOrder(2)]
        public bool Round
        {
            get => TransformEffect.Round;
            set => Model.SetTileProperty(TransformEffect, nameof(TransformEffect.Round), value);
        }

        public override void ModelPropertyChanged(string propertyName)
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
