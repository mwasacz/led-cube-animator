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
        public RotateEffectViewModel(RotateEffect rotateEffect, IModelManager model, GroupViewModel parent) : base(rotateEffect, model, parent) { }

        [Browsable(false)]
        public RotateEffect RotateEffect => (RotateEffect)Tile;

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

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == RotateEffect)
            {
                switch (propertyName)
                {
                    case nameof(RotateEffect.Axis):
                        changedProperty = nameof(Axis);
                        break;
                    case nameof(RotateEffect.Center):
                        changedProperty = nameof(Center);
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
