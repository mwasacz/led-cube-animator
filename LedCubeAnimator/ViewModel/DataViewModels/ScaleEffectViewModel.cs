using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("ScaleEffect", 3)]
    public class ScaleEffectViewModel : TransformEffectViewModel
    {
        public ScaleEffectViewModel(ScaleEffect scaleEffect, IModelManager model) : base(scaleEffect, model) { }

        public ScaleEffect ScaleEffect => (ScaleEffect)TransformEffect;

        [Category("ScaleEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => ScaleEffect.Axis;
            set => Model.SetTileProperty(ScaleEffect, nameof(ScaleEffect.Axis), value);
        }

        [Category("ScaleEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => ScaleEffect.Center;
            set => Model.SetTileProperty(ScaleEffect, nameof(ScaleEffect.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
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
