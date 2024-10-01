using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.TransformEffect))]
    public abstract class TransformEffectViewModel : EffectViewModel
    {
        public TransformEffectViewModel(TransformEffect transformEffect, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(transformEffect, model, messenger, parent) { }

        [Browsable(false)]
        public TransformEffect TransformEffect => (TransformEffect)Tile;

        [Category("TransformEffect")]
        [PropertyOrder(20)]
        public double From
        {
            get => TransformEffect.From;
            set => Model.SetTileProperty(TransformEffect, nameof(TransformEffect.From), value);
        }

        [Category("TransformEffect")]
        [PropertyOrder(21)]
        public double To
        {
            get => TransformEffect.To;
            set => Model.SetTileProperty(TransformEffect, nameof(TransformEffect.To), value);
        }

        [Category("TransformEffect")]
        [PropertyOrder(22)]
        public bool Round
        {
            get => TransformEffect.Round;
            set => Model.SetTileProperty(TransformEffect, nameof(TransformEffect.Round), value);
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
