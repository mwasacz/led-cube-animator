using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.GradientEffect))]
    public class GradientEffectViewModel : EffectViewModel
    {
        public GradientEffectViewModel(GradientEffect gradientEffect, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(gradientEffect, model, messenger, parent) { }

        [Browsable(false)]
        public GradientEffect GradientEffect => (GradientEffect)Tile;

        [Category("GradientEffect")]
        [PropertyOrder(20)]
        public Color From
        {
            get => GradientEffect.From;
            set => Model.SetTileProperty(GradientEffect, nameof(GradientEffect.From), value);
        }

        [Category("GradientEffect")]
        [PropertyOrder(21)]
        public Color To
        {
            get => GradientEffect.To;
            set => Model.SetTileProperty(GradientEffect, nameof(GradientEffect.To), value);
        }

        [Category("GradientEffect")]
        [PropertyOrder(22)]
        public ColorInterpolation ColorInterpolation
        {
            get => GradientEffect.ColorInterpolation;
            set => Model.SetTileProperty(GradientEffect, nameof(GradientEffect.ColorInterpolation), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(GradientEffect.From):
                    RaisePropertyChanged(nameof(From));
                    break;
                case nameof(GradientEffect.To):
                    RaisePropertyChanged(nameof(To));
                    break;
                case nameof(GradientEffect.ColorInterpolation):
                    RaisePropertyChanged(nameof(ColorInterpolation));
                    break;
            }
        }
    }
}
