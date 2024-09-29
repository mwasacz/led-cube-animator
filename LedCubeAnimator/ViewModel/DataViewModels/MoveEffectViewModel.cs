using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(moveEffect, model, messenger, parent) { }

        [Browsable(false)]
        public MoveEffect MoveEffect => (MoveEffect)Tile;

        [Category("MoveEffect")]
        [PropertyOrder(30)]
        public Axis Axis
        {
            get => MoveEffect.Axis;
            set => Model.SetTileProperty(MoveEffect, nameof(MoveEffect.Axis), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
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
