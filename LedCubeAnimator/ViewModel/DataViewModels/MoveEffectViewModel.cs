using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("MoveEffect", 3)]
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect, IModelManager model, GroupViewModel parent) : base(moveEffect, model, parent) { }

        [Browsable(false)]
        public MoveEffect MoveEffect => (MoveEffect)Tile;

        [Category("MoveEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => MoveEffect.Axis;
            set => Model.SetTileProperty(MoveEffect, nameof(MoveEffect.Axis), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == MoveEffect)
            {
                switch (propertyName)
                {
                    case nameof(MoveEffect.Axis):
                        changedProperty = nameof(Axis);
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
