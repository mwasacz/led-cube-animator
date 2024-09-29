﻿using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    public class RadialDelayViewModel : DelayViewModel
    {
        public RadialDelayViewModel(RadialDelay radialDelay, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(radialDelay, model, messenger, parent) { }

        [Browsable(false)]
        public RadialDelay RadialDelay => (RadialDelay)Tile;

        [Category("RadialDelay")]
        [PropertyOrder(20)]
        public Axis Axis
        {
            get => RadialDelay.Axis;
            set => Model.SetTileProperty(RadialDelay, nameof(RadialDelay.Axis), value);
        }

        [Category("RadialDelay")]
        [PropertyOrder(21)]
        public Point Center
        {
            get => RadialDelay.Center;
            set => Model.SetTileProperty(RadialDelay, nameof(RadialDelay.Center), GetNewValue(value, RadialDelay.Center));
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(RadialDelay.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(RadialDelay.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
