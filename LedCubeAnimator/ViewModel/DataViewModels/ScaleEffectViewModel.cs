﻿using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    public class ScaleEffectViewModel : TransformEffectViewModel
    {
        public ScaleEffectViewModel(ScaleEffect scaleEffect, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(scaleEffect, model, messenger, parent) { }

        [Browsable(false)]
        public ScaleEffect ScaleEffect => (ScaleEffect)Tile;

        [Category("ScaleEffect")]
        [PropertyOrder(30)]
        public Axis Axis
        {
            get => ScaleEffect.Axis;
            set => Model.SetTileProperty(ScaleEffect, nameof(ScaleEffect.Axis), value);
        }

        [Category("ScaleEffect")]
        [PropertyOrder(31)]
        public double Center
        {
            get => ScaleEffect.Center;
            set => Model.SetTileProperty(ScaleEffect, nameof(ScaleEffect.Center), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(ScaleEffect.Axis):
                    RaisePropertyChanged(nameof(Axis));
                    break;
                case nameof(ScaleEffect.Center):
                    RaisePropertyChanged(nameof(Center));
                    break;
            }
        }
    }
}
