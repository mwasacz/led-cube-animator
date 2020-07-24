using GalaSoft.MvvmLight;
using LedCubeAnimator.Model.Animations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel.WindowViewModels
{
    public class CubeSettingsViewModel : ViewModelBase
    {
        private int _size;
        public int Size
        {
            get => _size;
            set => Set(ref _size, value);
        }

        private ColorMode _colorMode;
        public ColorMode ColorMode
        {
            get => _colorMode;
            set
            {
                Set(ref _colorMode, value);
                if (_colorMode == ColorMode.RGB)
                {
                    MonoColor = Colors.White;
                }
            }
        }

        private Color _monoColor = Colors.Black;
        public Color MonoColor
        {
            get => _monoColor;
            set => Set(ref _monoColor, value);
        }

        private int _frameDuration;
        public int FrameDuration
        {
            get => _frameDuration;
            set => Set(ref _frameDuration, value);
        }
    }
}
