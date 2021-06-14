using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Animation", 3)]
    public class AnimationViewModel : GroupViewModel
    {
        public AnimationViewModel(Animation animation, IModelManager model, IViewModelFactory viewModelFactory) : base(animation, model, null, viewModelFactory) { }

        [Category("Animation")]
        [PropertyOrder(0)]
        public int Size
        {
            get => Animation.Size;
            set => Model.SetTileProperty(Animation, nameof(Animation.Size), value);
        }

        [Category("Animation")]
        [PropertyOrder(1)]
        public ColorMode ColorMode
        {
            get => Animation.ColorMode;
            set => Model.SetTileProperty(Animation, nameof(Animation.ColorMode), value);
        }

        [Category("Animation")]
        [PropertyOrder(2)]
        public Color MonoColor
        {
            get => Animation.MonoColor;
            set => Model.SetTileProperty(Animation, nameof(Animation.MonoColor), value);
        }

        [Category("Animation")]
        [PropertyOrder(3)]
        public int FrameDuration
        {
            get => Animation.FrameDuration;
            set => Model.SetTileProperty(Animation, nameof(Animation.FrameDuration), value);
        }

        [Browsable(false)]
        public Animation Animation => (Animation)Group;

        [Browsable(false)]
        public new int Start => base.Start;

        [Browsable(false)]
        public new int Channel => base.Channel;

        [Browsable(false)]
        public new int Hierarchy => base.Hierarchy;

        [Browsable(false)]
        public new bool Reverse => base.Reverse;

        [Browsable(false)]
        public new int RepeatCount => base.RepeatCount;

        [Browsable(false)]
        public new TimeInterpolation TimeInterpolation => base.TimeInterpolation;

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            base.ModelPropertyChanged(obj, propertyName, out changedViewModel, out changedProperty);
            if (obj == Animation)
            {
                switch (propertyName)
                {
                    case nameof(Animation.Size):
                        changedProperty = nameof(Size);
                        break;
                    case nameof(Animation.ColorMode):
                        changedProperty = nameof(ColorMode);
                        break;
                    case nameof(Animation.MonoColor):
                        changedProperty = nameof(MonoColor);
                        break;
                    case nameof(Animation.FrameDuration):
                        changedProperty = nameof(FrameDuration);
                        break;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
            }
        }
    }
}
