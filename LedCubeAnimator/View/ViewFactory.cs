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
                case MainViewModel main:
                    return typeof(MainWindow);
                case CubeSettingsViewModel cubeSettings:
                    return typeof(CubeSettingsDialog);
                default:
                    throw new Exception(); // ToDo
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
