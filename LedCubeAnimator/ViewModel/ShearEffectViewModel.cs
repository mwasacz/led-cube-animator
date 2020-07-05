using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("ShearEffect", 3)]
    public class ShearEffectViewModel : TransformEffectViewModel
    {
        public ShearEffectViewModel(ShearEffect shearEffect, UndoManager undo) : base(shearEffect, undo) { }

        public ShearEffect ShearEffect => (ShearEffect)TransformEffect;

        [Category("ShearEffect")]
        [PropertyOrder(0)]
        public Plane Plane
        {
            get => ShearEffect.Plane;
            set => Undo.Set(ShearEffect, nameof(ShearEffect.Plane), value);
        }

        [Category("ShearEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => ShearEffect.Center;
            set => Undo.Set(ShearEffect, nameof(ShearEffect.Center), value);
        }

        public override void ActionExecuted(IAction action)
        {
            base.ActionExecuted(action);
            if (action is PropertyChangeAction propertyAction && propertyAction.Object == ShearEffect)
            {
                switch (propertyAction.Property)
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
}
