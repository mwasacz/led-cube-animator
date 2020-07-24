using System.ComponentModel;
using System.Windows;

namespace LedCubeAnimator.View
{
    public interface IViewFactory
    {
        Window Create(INotifyPropertyChanged viewModel);
    }
}