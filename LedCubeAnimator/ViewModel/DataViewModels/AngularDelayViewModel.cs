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
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.AngularDelay))]
    public class AngularDelayViewModel : DelayViewModel
    {
        public AngularDelayViewModel(AngularDelay angularDelay, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(angularDelay, model, messenger, parent) { }

        [Browsable(false)]
        public AngularDelay AngularDelay => (AngularDelay)Tile;

        [Category("AngularDelay")]
        [PropertyOrder(20)]
        public Axis Axis
        {
            get => AngularDelay.Axis;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.Axis), value);
        }

        [Category("AngularDelay")]
        [PropertyOrder(21)]
        public Point Center
        {
            get => AngularDelay.Center;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.Center), GetNewValue(value, AngularDelay.Center));
        }

        [Category("AngularDelay")]
        [PropertyOrder(22)]
        public double StartAngle
        {
            get => AngularDelay.StartAngle;
            set => Model.SetTileProperty(AngularDelay, nameof(AngularDelay.StartAngle), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(AngularDelay.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(AngularDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
                case nameof(AngularDelay.StartAngle):
                    RaisePropertyChanged(nameof(StartAngle));
                    break;
            }
        }
    }
}
