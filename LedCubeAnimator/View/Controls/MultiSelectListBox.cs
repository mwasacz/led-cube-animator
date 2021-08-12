using System.Windows;
using System.Windows.Controls;

namespace LedCubeAnimator.View.Controls
{
    public class MultiSelectListBox : ListBox
    {
        static MultiSelectListBox()
        {
            SelectionModeProperty.OverrideMetadata(typeof(MultiSelectListBox), new FrameworkPropertyMetadata(SelectionMode.Extended));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiSelectListBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiSelectListBoxItem;
        }
    }
}
