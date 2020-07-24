using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("GradientEffect", 2)]
    public class GradientEffectViewModel : EffectViewModel
    {
        public GradientEffectViewModel(GradientEffect gradientEffect, IModelManager model) : base(gradientEffect, model) { }

        public GradientEffect GradientEffect => (GradientEffect)Effect;

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

        public override void ModelPropertyChanged(string propertyName)
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
