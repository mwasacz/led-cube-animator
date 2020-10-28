using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("GradientEffect", 2)]
    public class GradientEffectViewModel : EffectViewModel
    {
        public GradientEffectViewModel(GradientEffect gradientEffect, IModelManager model, GroupViewModel parent) : base(gradientEffect, model, parent) { }

        [Browsable(false)]
        public GradientEffect GradientEffect => (GradientEffect)Tile;

        [Category("GradientEffect")]
        [PropertyOrder(0)]
        public Color From
        {
            get => GradientEffect.From;
            set => Model.SetTileProperty(GradientEffect, nameof(GradientEffect.From), value);
        }

        [Category("GradientEffect")]
        [PropertyOrder(1)]
        public Color To
        {
            get => GradientEffect.To;
            set => Model.SetTileProperty(GradientEffect, nameof(GradientEffect.To), value);
        }

        [Category("GradientEffect")]
        [PropertyOrder(2)]
        public ColorInterpolation ColorInterpolation
        {
            get => GradientEffect.ColorInterpolation;
            set => Model.SetTileProperty(GradientEffect, nameof(GradientEffect.ColorInterpolation), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == GradientEffect)
            {
                switch (propertyName)
                {
                    case nameof(GradientEffect.From):
                        changedProperty = nameof(From);
                        break;
                    case nameof(GradientEffect.To):
                        changedProperty = nameof(To);
                        break;
                    case nameof(GradientEffect.ColorInterpolation):
                        changedProperty = nameof(ColorInterpolation);
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
