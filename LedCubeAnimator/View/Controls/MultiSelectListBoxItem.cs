using System.Windows.Controls;
using System.Windows.Input;

namespace LedCubeAnimator.View.Controls
{
    public class MultiSelectListBoxItem : ListBoxItem
    {
        private bool _deferDeselect;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsSelected)
            {
                _deferDeselect = true;
            }
            else
            {
                base.OnMouseLeftButtonDown(e);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_deferDeselect)
            {
                base.OnMouseLeftButtonDown(e);
                _deferDeselect = false;
            }
            base.OnMouseLeftButtonUp(e);
        }
    }
}