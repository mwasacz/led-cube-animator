using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Effect", 1)]
    public abstract class EffectViewModel : TileViewModel
    {
        public EffectViewModel(Effect effect, IModelManager model, GroupViewModel parent) : base(effect, model, parent) { }

        [Browsable(false)]
        public Effect Effect => (Effect)Tile;

        [Category("Effect")]
        [PropertyOrder(2)]
        public bool Reverse
        {
            get => Effect.Reverse;
            set => Model.SetTileProperty(Effect, nameof(Effect.Reverse), value);
        }

        [Category("Effect")]
        [PropertyOrder(1)]
        public int RepeatCount
        {
            get => Effect.RepeatCount;
            set => Model.SetTileProperty(Effect, nameof(Effect.RepeatCount), value);
        }

        [Category("Effect")]
        [PropertyOrder(0)]
        public TimeInterpolation TimeInterpolation
        {
            get => Effect.TimeInterpolation;
            set => Model.SetTileProperty(Effect, nameof(Effect.TimeInterpolation), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == Effect)
            {
                switch (propertyName)
                {
                    case nameof(Effect.Reverse):
                        changedProperty = nameof(Reverse);
                        break;
                    case nameof(Effect.RepeatCount):
                        changedProperty = nameof(RepeatCount);
                        break;
                    case nameof(Effect.TimeInterpolation):
                        changedProperty = nameof(TimeInterpolation);
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
