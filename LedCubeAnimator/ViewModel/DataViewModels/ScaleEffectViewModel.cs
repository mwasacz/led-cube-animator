using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("ScaleEffect", 3)]
    public class ScaleEffectViewModel : TransformEffectViewModel
    {
        public ScaleEffectViewModel(ScaleEffect scaleEffect, IModelManager model, GroupViewModel parent) : base(scaleEffect, model, parent) { }

        [Browsable(false)]
        public ScaleEffect ScaleEffect => (ScaleEffect)Tile;

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

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == ScaleEffect)
            {
                switch (propertyName)
                {
                    case nameof(ScaleEffect.Axis):
                        changedProperty = nameof(Axis);
                        break;
                    case nameof(ScaleEffect.Center):
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
