﻿// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.LinearDelay))]
    public class LinearDelayViewModel : DelayViewModel
    {
        public LinearDelayViewModel(LinearDelay linearDelay, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(linearDelay, model, messenger, parent) { }

        [Browsable(false)]
        public LinearDelay LinearDelay => (LinearDelay)Tile;

        [Category("LinearDelay")]
        [PropertyOrder(20)]
        public Axis Axis
        {
            get => LinearDelay.Axis;
            set => Model.SetTileProperty(LinearDelay, nameof(LinearDelay.Axis), value);
        }

        [Category("LinearDelay")]
        [PropertyOrder(21)]
        public double Center
        {
            get => LinearDelay.Center;
            set => Model.SetTileProperty(LinearDelay, nameof(LinearDelay.Center), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(LinearDelay.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(LinearDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
