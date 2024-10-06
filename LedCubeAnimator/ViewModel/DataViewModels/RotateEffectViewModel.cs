// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.RotateEffect))]
    public class RotateEffectViewModel : TransformEffectViewModel
    {
        public RotateEffectViewModel(RotateEffect rotateEffect, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(rotateEffect, model, messenger, parent) { }

        [Browsable(false)]
        public RotateEffect RotateEffect => (RotateEffect)Tile;

        [Category("RotateEffect")]
        [PropertyOrder(30)]
        public Axis Axis
        {
            get => RotateEffect.Axis;
            set => Model.SetTileProperty(RotateEffect, nameof(RotateEffect.Axis), value);
        }

        [Category("RotateEffect")]
        [PropertyOrder(31)]
        public Point Center
        {
            get => RotateEffect.Center;
            set => Model.SetTileProperty(RotateEffect, nameof(RotateEffect.Center), GetNewValue(value, RotateEffect.Center));
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
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
