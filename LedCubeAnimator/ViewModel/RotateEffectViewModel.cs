﻿using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("RotateEffect", 3)]
    public class RotateEffectViewModel : TransformEffectViewModel
    {
        public RotateEffectViewModel(RotateEffect rotateEffect, UndoManager undo) : base(rotateEffect, undo) { }

        public RotateEffect RotateEffect => (RotateEffect)TransformEffect;

        [Category("RotateEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => RotateEffect.Axis;
            set => Undo.Set(RotateEffect, nameof(RotateEffect.Axis), value);
        }

        [Category("RotateEffect")]
        [PropertyOrder(1)]
        public Point Center
        {
            get => RotateEffect.Center;
            set => Undo.Set(RotateEffect, nameof(RotateEffect.Center), value);
        }

        public override void ActionExecuted(IAction action)
        {
            base.ActionExecuted(action);
            if (action is PropertyChangeAction propertyAction && propertyAction.Object == RotateEffect)
            {
                switch (propertyAction.Property)
                {
                    case nameof(RotateEffect.Axis):
                        RaisePropertyChanged(nameof(Axis));
                        break;
                    case nameof(RotateEffect.Center):
                        RaisePropertyChanged(nameof(Center));
                        break;
                }
            }
        }
    }
}
