﻿using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("MoveEffect", 3)]
    public class MoveEffectViewModel : TransformEffectViewModel
    {
        public MoveEffectViewModel(MoveEffect moveEffect, UndoManager undo) : base(moveEffect, undo) { }

        public MoveEffect MoveEffect => (MoveEffect)TransformEffect;

        [Category("MoveEffect")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => MoveEffect.Axis;
            set => Undo.Set(MoveEffect, nameof(MoveEffect.Axis), value);
        }

        public override void ActionExecuted(IAction action)
        {
            base.ActionExecuted(action);
            if (action is PropertyChangeAction propertyAction && propertyAction.Object == MoveEffect)
            {
                switch (propertyAction.Property)
                {
                    case nameof(MoveEffect.Axis):
                        RaisePropertyChanged(nameof(Axis));
                        break;
                }
            }
        }
    }
}
