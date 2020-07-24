using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("ShearEffect", 3)]
    public class ShearEffectViewModel : TransformEffectViewModel
    {
        public ShearEffectViewModel(ShearEffect shearEffect, IModelManager model) : base(shearEffect, model) { }

        public ShearEffect ShearEffect => (ShearEffect)TransformEffect;

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

        public override void ModelPropertyChanged(string propertyName)
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
