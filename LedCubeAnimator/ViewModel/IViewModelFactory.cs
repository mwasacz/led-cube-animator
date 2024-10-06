// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using System.ComponentModel;

namespace LedCubeAnimator.ViewModel
{
    public interface IViewModelFactory
    {
        INotifyPropertyChanged Create(object model, params object[] args);
    }
}