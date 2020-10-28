using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.ViewModel.DataViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

        public int StartDate => 0;

        public int EndDate => Shared.Length - 1;

        private ObservableCollection<TileViewModel> _tiles;
        public ObservableCollection<TileViewModel> Tiles
        {
            get => _tiles;
            private set => Set(ref _tiles, value);
        }

        public ObservableCollection<GroupViewModel> Groups { get; } = new ObservableCollection<GroupViewModel>();

        private TileViewModel _selectedTile;
        public TileViewModel SelectedTile
        {
            get => _selectedTile;
            set
            {
                if (Set(ref _selectedTile, value) && _selectedTile != null)
                {
                    Shared.SelectedTile = value;
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
                    Shared.SelectedTile = value;
                }
            }
        }

        private void Shared_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISharedViewModel.SelectedTile) || e.PropertyName == nameof(ISharedViewModel.SelectedTileExpanded))
            {
                UpdateSelection();
            }
            else if (e.PropertyName == nameof(ISharedViewModel.Length))
            {
                RaisePropertyChanged(nameof(EndDate));
            }
        }

        private void UpdateSelection()
        {
            SelectedTile = Shared.SelectedTileExpanded ? null : Shared.SelectedTile;
            SelectedGroup = Shared.SelectedTileExpanded ? (GroupViewModel)Shared.SelectedTile : null;

            var group = SelectedGroup ?? SelectedTile.Parent;
            Tiles = group.ChildViewModels;

            int cnt = UpdateGroup(group);
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
