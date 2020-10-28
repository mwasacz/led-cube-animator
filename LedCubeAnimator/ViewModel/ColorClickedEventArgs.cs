using System;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel
{
    public class ColorClickedEventArgs : EventArgs
    {
        public ColorClickedEventArgs(Color color)
        {
            Color = color;
        }

        public Color Color { get; }
    }
}