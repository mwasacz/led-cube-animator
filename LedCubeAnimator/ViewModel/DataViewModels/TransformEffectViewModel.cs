using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("TransformEffect", 2)]
    public abstract class TransformEffectViewModel : EffectViewModel
    {
        public TransformEffectViewModel(TransformEffect transformEffect, IModelManager model, GroupViewModel parent) : base(transformEffect, model, parent) { }

        [Browsable(false)]
        public TransformEffect TransformEffect => (TransformEffect)Tile;

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

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == TransformEffect)
            {
                switch (propertyName)
                {
                    case nameof(TransformEffect.From):
                        changedProperty = nameof(From);
                        break;
                    case nameof(TransformEffect.To):
                        changedProperty = nameof(To);
                        break;
                    case nameof(TransformEffect.Round):
                        changedProperty = nameof(Round);
                        break;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
            }
        }
    }
}
