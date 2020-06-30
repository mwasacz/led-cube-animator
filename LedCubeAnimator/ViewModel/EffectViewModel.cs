using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
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
        public EffectViewModel(Effect effect, UndoManager undo) : base(effect, undo) { }

        public Effect Effect => (Effect)Tile;

        [Category("Effect")]
        [PropertyOrder(2)]
        public bool Reverse
        {
            get => Effect.Reverse;
            set => Undo.Set(Effect, nameof(Effect.Reverse), value);
        }

        [Category("Effect")]
        [PropertyOrder(1)]
        public int RepeatCount
        {
            get => Effect.RepeatCount;
            set => Undo.Set(Effect, nameof(Effect.RepeatCount), value);
        }

        [Category("Effect")]
        [PropertyOrder(0)]
        public TimeInterpolation TimeInterpolation
        {
            get => Effect.TimeInterpolation;
            set => Undo.Set(Effect, nameof(Effect.TimeInterpolation), value);
        }

        [Category("Effect")]
        [PropertyOrder(3)]
        public bool PersistEffect
        {
            get => Effect.PersistEffect;
            set => Undo.Set(Effect, nameof(Effect.PersistEffect), value);
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Effect.Reverse):
                    RaisePropertyChanged(nameof(Reverse));
                    break;
                case nameof(Effect.RepeatCount):
                    RaisePropertyChanged(nameof(RepeatCount));
                    break;
                case nameof(Effect.TimeInterpolation):
                    RaisePropertyChanged(nameof(TimeInterpolation));
                    break;
                case nameof(Effect.PersistEffect):
                    RaisePropertyChanged(nameof(PersistEffect));
                    break;
            }
        }
    }
}
