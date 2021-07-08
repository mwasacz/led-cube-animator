using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Group", 2)]
    public class GroupViewModel : EffectViewModel
    {
        public GroupViewModel(Group group, IModelManager model, GroupViewModel parent, IViewModelFactory viewModelFactory) : base(group, model, parent)
        {
            _viewModelFactory = viewModelFactory;
            UpdateColumns();
            UpdateChildren();
            UpdateRows();
        }

        [Browsable(false)]
        public Group Group => (Group)Tile;

        private readonly IViewModelFactory _viewModelFactory;

        [Category("Group")]
        [PropertyOrder(0)]
        public object Children { get; }

        [Category("Group")]
        [PropertyOrder(1)]
        public ColorBlendMode ColorBlendMode
        {
            get => Group.ColorBlendMode;
            set => Model.SetTileProperty(Group, nameof(Group.ColorBlendMode), value);
        }

        [Browsable(false)]
        public ObservableCollection<TileViewModel> ChildViewModels { get; } = new ObservableCollection<TileViewModel>();

        private int _columns;

        [Browsable(false)]
        public int Columns
        {
            get => _columns;
            private set => Set(ref _columns, value);
        }

        private int _rows;

        [Browsable(false)]
        public int Rows
        {
            get => _rows;
            private set => Set(ref _rows, value);
        }

        [Browsable(false)]
        public ObservableCollection<int> RowHeights { get; } = new ObservableCollection<int>();

        public override void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            changedViewModel = null;
            changedProperty = null;

            foreach (var tile in ChildViewModels)
            {
                tile.ModelPropertyChanged(obj, propertyName, out var childViewModel, out var childProperty);
                if (childViewModel != null)
                {
                    changedViewModel = childViewModel;
                    changedProperty = childProperty;
                }
            }

            base.ModelPropertyChanged(obj, propertyName, out var baseViewModel, out var baseProperty);
            if (baseViewModel != null)
            {
                changedViewModel = baseViewModel;
                changedProperty = baseProperty;
            }

            if (obj == Group)
            {
                switch (propertyName)
                {
                    case nameof(Group.Children):
                        changedProperty = nameof(Children);
                        break;
                    case nameof(Group.ColorBlendMode):
                        changedProperty = nameof(ColorBlendMode);
                        break;
                    case nameof(Group.Start):
                    case nameof(Group.End):
                    case nameof(Group.RepeatCount):
                    case nameof(Group.Reverse):
                        UpdateColumns();
                        return;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
                if (propertyName == nameof(Group.Children))
                {
                    UpdateChildren();
                    UpdateRows();
                }
            }
            else if (Group.Children.Contains(obj) && (propertyName == nameof(Tile.Start)
                || propertyName == nameof(Tile.End)
                || propertyName == nameof(Tile.Hierarchy)
                || propertyName == nameof(Tile.Channel)))
            {
                UpdateRows();
            }
        }

        private void UpdateChildren()
        {
            foreach (var tile in Group.Children)
            {
                if (!ChildViewModels.Any(c => c.Tile == tile))
                {
                    ChildViewModels.Add((TileViewModel)_viewModelFactory.Create(tile, this));
                }
            }
            for (int i = ChildViewModels.Count - 1; i >= 0; i--)
            {
                if (!Group.Children.Contains(ChildViewModels[i].Tile))
                {
                    ChildViewModels.RemoveAt(i);
                }
            }
        }

        private void UpdateColumns()
        {
            Columns = Reverse ? Length / RepeatCount / 2 : Length / RepeatCount;
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
                RowHeights.RemoveAt(i - 1);
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
