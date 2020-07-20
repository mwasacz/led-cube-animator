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
    [CategoryOrder("LinearDelay", 2)]
    public class LinearDelayViewModel : DelayViewModel
    {
        public LinearDelayViewModel(LinearDelay linearDelay, IModelManager model) : base(linearDelay, model) { }

        public LinearDelay LinearDelay => (LinearDelay)Delay;

        [Category("LinearDelay")]
        [PropertyOrder(0)]
        public Axis Axis
        {
            get => LinearDelay.Axis;
            set => Model.SetTileProperty(LinearDelay, nameof(LinearDelay.Axis), value);
        }

        [Category("LinearDelay")]
        [PropertyOrder(1)]
        public double Center
        {
            get => LinearDelay.Center;
            set => Model.SetTileProperty(LinearDelay, nameof(LinearDelay.Center), value);
        }

        public override void ModelPropertyChanged(string propertyName)
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
