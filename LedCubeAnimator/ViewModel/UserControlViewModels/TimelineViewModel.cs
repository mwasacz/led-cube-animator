﻿// This file is part of LED Cube Animator
// Copyright (C) 2019-2021, 2024 Mikolaj Wasacz
// SPDX-License-Identifier: GPL-3.0-only WITH GPL-3.0-linking-source-exception

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LedCubeAnimator.Model;
using LedCubeAnimator.Utils;
using LedCubeAnimator.ViewModel.DataViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace LedCubeAnimator.ViewModel.UserControlViewModels
{
    public class TimelineViewModel : ViewModelBase
    {
        public TimelineViewModel(IModelManager model, ISharedViewModel shared)
        {
            Model = model;
            Shared = shared;

            Shared.PropertyChanged += Shared_PropertyChanged;

            UpdateGroups();
        }

        public IModelManager Model { get; }
        public ISharedViewModel Shared { get; }

        public ObservableCollection<GroupViewModel> Groups { get; } = new ObservableCollection<GroupViewModel>();

        private RelayCommand<ItemDraggedEventArgs> _itemDraggedCommand;
        public ICommand ItemDraggedCommand => _itemDraggedCommand ?? (_itemDraggedCommand = new RelayCommand<ItemDraggedEventArgs>(e =>
        {
            Model.Group(() =>
            {
                var anchor = (TileViewModel)e.Item;
                foreach (var tile in Shared.SelectedTiles)
                {
                    int groupLength = tile.Parent?.Columns ?? int.MaxValue;
                    switch (e.DragMode)
                    {
                        case DragMode.Left:
                            tile.Start = Math.Max(Math.Min(e.PositionX, tile.End), 0);
                            break;
                        case DragMode.Right:
                            tile.End = Math.Min(Math.Max(e.PositionX, tile.Start), groupLength - 1);
                            break;
                        case DragMode.Move:
                            int length = tile.Length;
                            int start = Math.Max(Math.Min(e.PositionX - e.HandleOffset - anchor.Start + tile.Start, groupLength - length), 0);
                            tile.Start = start;
                            tile.End = start + length - 1;

                            int row = e.PositionY;
                            int channel = 0;
                            while (channel < anchor.Parent?.RowHeights.Count && row >= anchor.Parent.RowHeights[channel])
                            {
                                row -= anchor.Parent.RowHeights[channel];
                                channel++;
                            }
                            tile.Channel = channel;
                            tile.Hierarchy = row - anchor.Hierarchy + tile.Hierarchy;
                            break;
                    }
                }
            });
        }));

        private void Shared_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISharedViewModel.CurrentGroup))
            {
                UpdateGroups();
            }
        }

        private void UpdateGroups()
        {
            int cnt = UpdateGroup(Shared.CurrentGroup);
            while (Groups.Count > cnt)
            {
                Groups.RemoveAt(Groups.Count - 1);
            }
        }

        private int UpdateGroup(GroupViewModel group)
        {
            int i = group.Parent == null ? 0 : UpdateGroup(group.Parent);
            if (i >= Groups.Count)
            {
                Groups.Add(group);
            }
            else if (Groups[i] != group)
            {
                Groups.Insert(i, group);
            }
            return i + 1;
        }
    }
}
