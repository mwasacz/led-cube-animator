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
            OrderChildren();
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
                        OrderChildren();
                        changedProperty = nameof(Children);
                        break;
                    case nameof(Group.ColorBlendMode):
                        changedProperty = nameof(ColorBlendMode);
                        break;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
            }
            else if (Group.Children.Contains(obj) && (propertyName == nameof(Tile.Channel)
                  || propertyName == nameof(Tile.Start)
                  || propertyName == nameof(Tile.Hierarchy)))
            {
                OrderChildren();
            }
        }

        private void OrderChildren()
        {
            int i = 0;
            foreach (var tile in Group.Children.OrderBy(t => t.Channel).ThenBy(t => t.Start).ThenBy(t => t.Hierarchy))
            {
                if (i >= ChildViewModels.Count || ChildViewModels[i].Tile != tile)
                {
                    int index = ChildViewModels.TakeWhile(t => t.Tile != tile).Count();
                    if (index < ChildViewModels.Count)
                    {
                        ChildViewModels.Move(index, i);
                    }
                    else
                    {
                        var child = (TileViewModel)_viewModelFactory.Create(tile, this);
                        ChildViewModels.Insert(i, child);
                    }
                }
                i++;
            }
            while (i < ChildViewModels.Count)
            {
                var child = ChildViewModels[ChildViewModels.Count - 1];
                ChildViewModels.RemoveAt(ChildViewModels.Count - 1);
            }
        }
    }
}
