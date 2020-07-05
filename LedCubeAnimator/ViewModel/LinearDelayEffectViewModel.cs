﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("LinearDelayEffect", 2)]
    public class LinearDelayEffectViewModel : EffectViewModel
    {
        public LinearDelayEffectViewModel(LinearDelayEffect linearDelayEffect, UndoManager undo) : base(linearDelayEffect, undo) { }

        public LinearDelayEffect LinearDelayEffect => (LinearDelayEffect)Effect;

        [Category("LinearDelayEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => LinearDelayEffect.Axis;
            set => Undo.Set(LinearDelayEffect, nameof(LinearDelayEffect.Axis), value);
        }

        [Category("LinearDelayEffect")]
        [PropertyOrder(1)]
        public double Center
        {
            get => LinearDelayEffect.Center;
            set => Undo.Set(LinearDelayEffect, nameof(LinearDelayEffect.Center), value);
        }

        [Category("LinearDelayEffect")]
        [PropertyOrder(2)]
        public double Value
        {
            get => LinearDelayEffect.Value;
            set => Undo.Set(LinearDelayEffect, nameof(LinearDelayEffect.Value), value);
        }

        public override void ActionExecuted(IAction action)
        {
            base.ActionExecuted(action);
            if (action is PropertyChangeAction propertyAction && propertyAction.Object == LinearDelayEffect)
            {
                switch (propertyAction.Property)
                {
                    case nameof(LinearDelayEffect.Axis):
                        RaisePropertyChanged(nameof(Axis));
                        break;
                    case nameof(LinearDelayEffect.Center):
                        RaisePropertyChanged(nameof(Center));
                        break;
                    case nameof(LinearDelayEffect.Value):
                        RaisePropertyChanged(nameof(Value));
                        break;
                }
            }
        }
    }
}
