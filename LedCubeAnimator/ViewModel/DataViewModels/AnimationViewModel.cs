using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    public class AnimationViewModel : GroupViewModel
    {
        public AnimationViewModel(Animation animation, IModelManager model, IMessenger messenger, IViewModelFactory viewModelFactory) : base(animation, model, messenger, null, viewModelFactory) { }

        [Browsable(false)]
        public Animation Animation => (Animation)Group;

        [Category("Animation")]
        [PropertyOrder(30)]
        [Range(1, int.MaxValue)]
        public int Size
        {
            get => Animation.Size;
            set => Model.SetTileProperty(Animation, nameof(Animation.Size), value);
        }

        [Category("Animation")]
        [PropertyOrder(31)]
        public ColorMode ColorMode
        {
            get => Animation.ColorMode;
            set => Model.SetTileProperty(Animation, nameof(Animation.ColorMode), value);
        }

        [Category("Animation")]
        [PropertyOrder(32)]
        public Color MonoColor
        {
            get => Animation.MonoColor;
            set => Model.SetTileProperty(Animation, nameof(Animation.MonoColor), value);
        }

        [Category("Animation")]
        [PropertyOrder(33)]
        [Range(1, int.MaxValue)]
        public int FrameDuration
        {
            get => Animation.FrameDuration;
            set => Model.SetTileProperty(Animation, nameof(Animation.FrameDuration), value);
        }

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

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Animation.Size):
                    RaisePropertyChanged(nameof(Size));
                    break;
                case nameof(Animation.ColorMode):
                    RaisePropertyChanged(nameof(ColorMode));
                    break;
                case nameof(Animation.MonoColor):
                    RaisePropertyChanged(nameof(MonoColor));
                    break;
                case nameof(Animation.FrameDuration):
                    RaisePropertyChanged(nameof(FrameDuration));
                    break;
            }
        }
    }
}
