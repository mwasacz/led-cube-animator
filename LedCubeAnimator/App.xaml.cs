﻿// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.View;
using LedCubeAnimator.ViewModel;
using LedCubeAnimator.ViewModel.WindowViewModels;
using MvvmDialogs;
using System.Windows;

namespace LedCubeAnimator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var modelManager = new ModelManager();
            var messenger = new Messenger();
            var viewModelFactory = new ViewModelFactory(modelManager, messenger);
            var viewFactory = new ViewFactory();
            var dialogService = new DialogService(dialogTypeLocator: viewFactory);

            var mainViewModel = new MainViewModel(modelManager, messenger, viewModelFactory, dialogService);
            var mainWindow = viewFactory.Create(mainViewModel);
            mainWindow.Show();
        }
    }
}
