// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using LedCubeAnimator.ViewModel.DataViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel
{
    public interface ISharedViewModel : INotifyPropertyChanged
    {
        GroupViewModel CurrentGroup { get; }
        ObservableCollection<TileViewModel> SelectedTiles { get; }
        string SelectedProperty { get; set; }

        Color? SelectedColor { get; set; }
        void ColorClick(Color color);
        event EventHandler<ColorClickedEventArgs> ColorClicked;

        int Time { get; set; }
    }
}
