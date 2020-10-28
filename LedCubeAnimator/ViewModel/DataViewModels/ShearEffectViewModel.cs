using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("ShearEffect", 3)]
    public class ShearEffectViewModel : TransformEffectViewModel
    {
        public ShearEffectViewModel(ShearEffect shearEffect, IModelManager model, GroupViewModel parent) : base(shearEffect, model, parent) { }

        [Browsable(false)]
        public ShearEffect ShearEffect => (ShearEffect)Tile;

        [Category("ShearEffect")]
        [PropertyOrder(0)]
        public Plane Plane
        {
            get => ShearEffect.Plane;
            set => Model.SetTileProperty(ShearEffect, nameof(ShearEffect.Plane), value);
        }

        [Category("ShearEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => ShearEffect.Center;
            set => Model.SetTileProperty(ShearEffect, nameof(ShearEffect.Center), value);
        }

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == ShearEffect)
            {
                switch (propertyName)
                {
                    case nameof(ShearEffect.Plane):
                        changedProperty = nameof(Plane);
                        break;
                    case nameof(ShearEffect.Center):
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
