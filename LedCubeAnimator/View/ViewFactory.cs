using LedCubeAnimator.View.Windows;
using LedCubeAnimator.ViewModel.WindowViewModels;
using MvvmDialogs.DialogTypeLocators;
using System;
using System.ComponentModel;
using System.Windows;

namespace LedCubeAnimator.View
{
    public class ViewFactory : IViewFactory, IDialogTypeLocator
    {
        public Type Locate(INotifyPropertyChanged viewModel)
        {
            switch (viewModel)
            {
                case MainViewModel _:
                    return typeof(MainWindow);
                case CubeSettingsViewModel _:
                    return typeof(CubeSettingsDialog);
                default:
                    throw new ArgumentException("Could not find suitable view for this type of viewModel", "viewModel");
            }
        }

        public Window Create(INotifyPropertyChanged viewModel)
        {
            var type = Locate(viewModel);
            var window = (Window)Activator.CreateInstance(type);
            window.DataContext = viewModel;
            return window;
        }
    }
}
