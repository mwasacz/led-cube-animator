// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [DisplayName(nameof(LedCubeAnimator.Model.Animations.Data.SphericalDelay))]
    public class SphericalDelayViewModel : DelayViewModel
    {
        public SphericalDelayViewModel(SphericalDelay sphericalDelay, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(sphericalDelay, model, messenger, parent) { }

        [Browsable(false)]
        public SphericalDelay SphericalDelay => (SphericalDelay)Tile;

        [Category("SphericalDelay")]
        [PropertyOrder(20)]
        public Point3D Center
        {
            get => SphericalDelay.Center;
            set => Model.SetTileProperty(SphericalDelay, nameof(SphericalDelay.Center), GetNewValue(value, SphericalDelay.Center));
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SphericalDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
