using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
                foreach (var tile in Shared.SelectedTiles)
                {
                    switch (e.DragMode)
                    {
                        case DragMode.Left:
                            tile.Start = e.PositionX;
                            break;
                        case DragMode.Right:
                            tile.End = e.PositionX;
                            break;
                        case DragMode.Move:
                            var start = Math.Max(e.PositionX - e.HandleOffset, 0);
                            var length = tile.Length;
                            tile.Start = start;
                            tile.End = start + length - 1;

                            int row = e.PositionY;
                            int channel = 0;
                            while (channel < tile.Parent?.RowHeights?.Count && row >= tile.Parent.RowHeights[channel])
                            {
                                row -= tile.Parent.RowHeights[channel];
                                channel++;
                            }
                            tile.Channel = channel;
                            tile.Hierarchy = row;
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
