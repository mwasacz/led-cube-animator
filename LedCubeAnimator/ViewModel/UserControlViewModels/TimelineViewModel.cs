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

            UpdateSelection();
        }

        public IModelManager Model { get; }
        public ISharedViewModel Shared { get; }

        private GroupViewModel _currentGroup;
        public GroupViewModel CurrentGroup
        {
            get => _currentGroup;
            private set
            {
                if (Set(ref _currentGroup, value))
                {
                    UpdateGroups();
                }
            }
        }

        public ObservableCollection<GroupViewModel> Groups { get; } = new ObservableCollection<GroupViewModel>();

        private TileViewModel _selectedTile;
        public TileViewModel SelectedTile
        {
            get => _selectedTile;
            set
            {
                if (Set(ref _selectedTile, value))
                {
                    if (_selectedTile != null)
                    {
                        Shared.SelectedTile = _selectedTile;
                    }
                    else
                    {
                        SelectedGroup = CurrentGroup;
                    }
                }
            }
        }

        private GroupViewModel _selectedGroup;
        public GroupViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (Set(ref _selectedGroup, value) && _selectedGroup != null)
                {
                    Shared.SelectedTile = _selectedGroup;
                }
            }
        }

        private RelayCommand<ItemDraggedEventArgs> _itemDraggedCommand;
        public ICommand ItemDraggedCommand => _itemDraggedCommand ?? (_itemDraggedCommand = new RelayCommand<ItemDraggedEventArgs>(e =>
        {
            var tile = SelectedTile;
            switch (e.DragMode)
            {
                case DragMode.Left:
                    tile.Start = e.PositionX;
                    break;
                case DragMode.Right:
                    tile.End = e.PositionX;
                    break;
                case DragMode.Move:
                    Model.Group(() =>
                    {
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
                    });
                    break;
            }
        }));

        private void Shared_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISharedViewModel.SelectedTile) || e.PropertyName == nameof(ISharedViewModel.SelectedTileExpanded))
            {
                UpdateSelection();
            }
        }

        private void UpdateSelection()
        {
            if (Shared.SelectedTileExpanded)
            {
                CurrentGroup = (GroupViewModel)Shared.SelectedTile;
                SelectedGroup = (GroupViewModel)Shared.SelectedTile;
                SelectedTile = null;
            }
            else
            {
                CurrentGroup = Shared.SelectedTile.Parent;
                SelectedGroup = null;
                SelectedTile = Shared.SelectedTile;
            }
        }

        private void UpdateGroups()
        {
            int cnt = UpdateGroup(_currentGroup);
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
