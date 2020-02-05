using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("Effect", 1)]
    public abstract class EffectViewModel : TileViewModel
    {
        public EffectViewModel(Effect effect) : base(effect) { }

        public Effect Effect => (Effect)Tile;

        [Category("Effect")]
        [PropertyOrder(2)]
        public bool Reverse
        {
            get => Effect.Reverse;
            set
            {
                Effect.Reverse = value;
                RaisePropertyChanged(nameof(Reverse));
            }
        }

        [Category("Effect")]
        [PropertyOrder(1)]
        public int RepeatCount
        {
            get => Effect.RepeatCount;
            set
            {
                Effect.RepeatCount = value;
                RaisePropertyChanged(nameof(RepeatCount));
            }
        }

        [Category("Effect")]
        [PropertyOrder(0)]
        public TimeInterpolation TimeInterpolation
        {
            get => Effect.TimeInterpolation;
            set
            {
                Effect.TimeInterpolation = value;
                RaisePropertyChanged(nameof(TimeInterpolation));
            }
        }

        [Category("Effect")]
        [PropertyOrder(3)]
        public bool PersistEffect
        {
            get => Effect.PersistEffect;
            set
            {
                Effect.PersistEffect = value;
                RaisePropertyChanged(nameof(PersistEffect));
            }
        }
    }
}
