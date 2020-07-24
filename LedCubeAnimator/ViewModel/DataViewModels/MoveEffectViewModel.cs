using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("MoveEffect", 3)]
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect, IModelManager model) : base(moveEffect, model) { }

        public MoveEffect MoveEffect => (MoveEffect)TransformEffect;

        [Category("MoveEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => MoveEffect.Axis;
            set => Model.SetTileProperty(MoveEffect, nameof(MoveEffect.Axis), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(MoveEffect.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
            }
        }
    }
}
