using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel
{
    public class AnimationViewModel : ViewModelBase
    {
        public AnimationViewModel(Animation animation)
        {
            Animation = animation;
        }

        public Animation Animation { get; }

        public Group MainGroup
        {
            get => Animation.MainGroup;
            set
            {
                Animation.MainGroup = value;
                RaisePropertyChanged(nameof(MainGroup));
            }
        }

        public int Size
        {
            get => Animation.Size;
            set
            {
                Animation.Size = value;
                RaisePropertyChanged(nameof(Size));
            }
        }

        public ColorMode ColorMode
        {
            get => Animation.ColorMode;
            set
            {
                Animation.ColorMode = value;
                RaisePropertyChanged(nameof(ColorMode));
            }
        }

        public Color MonoColor
        {
            get => Animation.MonoColor;
            set
            {
                Animation.MonoColor = value;
                RaisePropertyChanged(nameof(MonoColor));
            }
        }
    }
}
