using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    public class GroupViewModel : EffectViewModel
    {
        public GroupViewModel(Group group, IModelManager model, IMessenger messenger, GroupViewModel parent, IViewModelFactory viewModelFactory) : base(group, model, messenger, parent)
        {
            _viewModelFactory = viewModelFactory;

            Model.PropertiesChanged += Model_PropertiesChanged;

            UpdateColumns();
            UpdateChildren();
            UpdateRows();
        }

        private readonly IViewModelFactory _viewModelFactory;
        private int _columns;
        private int _rows;
        private bool _expanded;

        [Browsable(false)]
        public Group Group => (Group)Tile;

        [Category("Group")]
        [PropertyOrder(20)]
        public string Children => Expanded ? "Edit on Timeline" : "Click to edit";

        [Category("Group")]
        [PropertyOrder(21)]
        public ColorBlendMode ColorBlendMode
        {
            get => Group.ColorBlendMode;
            set => Model.SetTileProperty(Group, nameof(Group.ColorBlendMode), value);
        }

        [Browsable(false)]
        public ObservableCollection<TileViewModel> ChildViewModels { get; } = new ObservableCollection<TileViewModel>();

        [Browsable(false)]
        public int Columns
        {
            get => _columns;
            private set => Set(ref _columns, value);
        }

        [Browsable(false)]
        public int Rows
        {
            get => _rows;
            private set => Set(ref _rows, value);
        }

        [Browsable(false)]
        public ObservableCollection<int> RowHeights { get; } = new ObservableCollection<int>();

        [Browsable(false)]
        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (Set(ref _expanded, value, true))
                {
                    RaisePropertyChanged(nameof(Children));
                    if (!_expanded)
                    {
                        foreach (var group in ChildViewModels.OfType<GroupViewModel>())
                        {
                            group.Expanded = false;
                        }
                    }
                }
            }
        }

        public override void Cleanup()
        {
            foreach (var c in ChildViewModels)
            {
                c.Cleanup();
            }
            Selected = false;
            Expanded = false;
            Model.PropertiesChanged -= Model_PropertiesChanged;
            base.Cleanup();
        }

        protected override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Group.Children):
                    RaisePropertyChanged(nameof(Children));
                    UpdateChildren();
                    UpdateRows();
                    break;
                case nameof(Group.ColorBlendMode):
                    RaisePropertyChanged(nameof(ColorBlendMode));
                    break;
                case nameof(Group.Start):
                case nameof(Group.End):
                case nameof(Group.RepeatCount):
                case nameof(Group.Reverse):
                    UpdateColumns();
                    break;
            }
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            if (e.Changes.Any(c => Group.Children.Contains(c.Key)
                && (c.Value == nameof(Tile.Start)
                    || c.Value == nameof(Tile.End)
                    || c.Value == nameof(Tile.Hierarchy)
                    || c.Value == nameof(Tile.Channel))))
            {
                UpdateRows();
            }
        }

        private void UpdateColumns()
        {
            Columns = Reverse ? Length / RepeatCount / 2 : Length / RepeatCount;
        }

        private void UpdateChildren()
        {
            foreach (var tile in Group.Children)
            {
                if (!ChildViewModels.Any(c => c.Tile == tile))
                {
                    var viewModel = (TileViewModel)_viewModelFactory.Create(tile, this);
                    ChildViewModels.Add(viewModel);
                }
            }
            for (int i = ChildViewModels.Count - 1; i >= 0; i--)
            {
                var viewModel = ChildViewModels[i];
                if (!Group.Children.Contains(viewModel.Tile))
                {
                    ChildViewModels.RemoveAt(i);
                    viewModel.Cleanup();
                }
            }
        }

        private void UpdateRows()
        {
            int maxChannel = Group.Children.Select(c => c.Channel).DefaultIfEmpty().Max();
            var rowHeights = Enumerable.Range(0, maxChannel + 1)
                .Select(x => Group.Children.Where(c => c.Channel == x).Select(c => c.Hierarchy + 1).DefaultIfEmpty().Max() + 1)
                .ToArray();

            int i;
            for (i = 0; i < rowHeights.Length; i++)
            {
                if (i < RowHeights.Count)
                {
                    RowHeights[i] = rowHeights[i];
                }
                else
                {
                    RowHeights.Add(rowHeights[i]);
                }
            }
            while (i < RowHeights.Count)
            {
                RowHeights.RemoveAt(RowHeights.Count - 1);
            }

            int sum = 0;
            var rowPositions = new int[rowHeights.Length];
            for (int j = 0; j < rowHeights.Length; j++)
            {
                rowPositions[j] = sum;
                sum += rowHeights[j];
            }
            Rows = sum;

            foreach (var c in ChildViewModels)
            {
                c.Row = rowPositions[c.Channel] + c.Hierarchy;
            }
        }
    }
}
