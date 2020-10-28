using System.ComponentModel;

namespace LedCubeAnimator.ViewModel
{
    public interface IViewModelFactory
    {
        INotifyPropertyChanged Create(object model, params object[] args);
    }
}