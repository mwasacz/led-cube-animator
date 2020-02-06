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

            _mainGroup = new GroupViewModel(animation.MainGroup);
        }

        public Animation Animation { get; }

        private GroupViewModel _mainGroup;

        public GroupViewModel MainGroup
        {
            get => _mainGroup;
            set
            {
                _mainGroup = value;
                Animation.MainGroup = _mainGroup.Group;
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
                if (Animation.ColorMode != ColorMode.RGB)
                {
                    SetGroupColorMode(MainGroup, Animation.ColorMode);
                }
                RaisePropertyChanged(nameof(ColorMode));
            }
        }

        public void SetGroupColorMode(GroupViewModel group, ColorMode colorMode)
        {
            foreach (var t in group.Children)
            {
                switch (t)
                {
                    case FrameViewModel fr:
                        for (int i = 0; i < fr.Voxels.Count; i++)
                        {
                            Color oldColor = fr.Voxels[i];
                            Color newColor = default;
                            switch (colorMode)
                            {
                                case ColorMode.Mono:
                                    newColor = oldColor.GetBrightness() > 127 ? Colors.White : Colors.Black;
                                    break;
                                case ColorMode.MonoBrightness:
                                    newColor = Colors.White.Multiply(oldColor.GetBrightness()).Opaque();
                                    break;
                            }
                            if (newColor != oldColor)
                            {
                                fr.Voxels[i] = newColor;
                            }
                        }
                        break;
                    case GroupViewModel gr:
                        SetGroupColorMode(gr, colorMode);
                        break;
                }
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
