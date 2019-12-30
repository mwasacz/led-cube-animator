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
    public class CubeSettingsViewModel : ViewModelBase
    {
        private int _size;
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                RaisePropertyChanged(nameof(Size));
            }
        }

        private ColorMode _colorMode;
        public ColorMode ColorMode
        {
            get => _colorMode;
            set
            {
                _colorMode = value;
                if (_colorMode == ColorMode.RGB)
                {
                    MonoColor = Colors.White;
                }
                RaisePropertyChanged(nameof(ColorMode));
            }
        }

        private Color _monoColor = Colors.Black;
        public Color MonoColor
        {
            get => _monoColor;
            set
            {
                _monoColor = value;
                RaisePropertyChanged(nameof(MonoColor));
            }
        }
    }
}
