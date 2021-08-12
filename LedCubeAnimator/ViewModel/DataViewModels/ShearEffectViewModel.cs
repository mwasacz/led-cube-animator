using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("ShearEffect", 3)]
    public class ShearEffectViewModel : TransformEffectViewModel
    {
        public ShearEffectViewModel(ShearEffect shearEffect, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(shearEffect, model, messenger, parent) { }

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

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(ShearEffect.Plane):
                    RaisePropertyChanged(nameof(Plane));
                    break;
                case nameof(ShearEffect.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
