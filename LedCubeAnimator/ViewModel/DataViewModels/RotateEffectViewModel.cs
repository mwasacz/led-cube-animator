using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("RotateEffect", 3)]
    public class RotateEffectViewModel : TransformEffectViewModel
    {
        public RotateEffectViewModel(RotateEffect rotateEffect, IModelManager model) : base(rotateEffect, model) { }

        public RotateEffect RotateEffect => (RotateEffect)TransformEffect;

        [Category("RotateEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => RotateEffect.Axis;
            set => Model.SetTileProperty(RotateEffect, nameof(RotateEffect.Axis), value);
        }

        [Category("RotateEffect")]
        [PropertyOrder(1)]
        public Point Center
        {
            get => RotateEffect.Center;
            set => Model.SetTileProperty(RotateEffect, nameof(RotateEffect.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(RotateEffect.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(RotateEffect.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
