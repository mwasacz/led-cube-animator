// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

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
