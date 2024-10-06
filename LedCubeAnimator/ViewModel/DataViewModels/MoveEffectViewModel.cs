// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.MoveEffect))]
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
