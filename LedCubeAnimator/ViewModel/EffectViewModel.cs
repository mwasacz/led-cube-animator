using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.ViewModel
{
    public abstract class EffectViewModel : TileViewModel
    {
        public EffectViewModel(Effect effect) : base(effect) { }

        public Effect Effect => (Effect)Tile;

        public Direction Direction
        {
            get => Effect.Direction;
            set
            {
                Effect.Direction = value;
                RaisePropertyChanged(nameof(Direction));
            }
        }

        public int RepeatCount
        {
            get => Effect.RepeatCount;
            set
            {
                Effect.RepeatCount = value;
                RaisePropertyChanged(nameof(RepeatCount));
            }
        }

        public TimeInterpolation TimeInterpolation
        {
            get => Effect.TimeInterpolation;
            set
            {
                Effect.TimeInterpolation = value;
                RaisePropertyChanged(nameof(TimeInterpolation));
            }
        }

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
