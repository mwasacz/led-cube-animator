using GalaSoft.MvvmLight.Command;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("Group", 2)]
    public class GroupViewModel : EffectViewModel
    {
        public GroupViewModel(Group group) : base(group)
        {
            ChildrenCollection = new ObservableCollection<TileViewModel>(group.Children.Select(CreateViewModel));

            ChildrenCollection.CollectionChanged += ChildrenCollection_CollectionChanged;
        }

        public Group Group => (Group)Effect;

        private RelayCommand _editChildren;

        [Category("Group")]
        [PropertyOrder(0)]
        public ICommand Children => _editChildren ?? (_editChildren = new RelayCommand(() => EditChildren?.Invoke(this, EventArgs.Empty))); // ToDo

        public event EventHandler EditChildren;

        public ObservableCollection<TileViewModel> ChildrenCollection { get; }

        private void ChildrenCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Group.Children.Clear();
            Group.Children.AddRange(ChildrenCollection.Select(c => c.Tile));
        }

        private TileViewModel CreateViewModel(Tile tile)
        {
            switch (tile)
            {
                case Frame frame:
                    return new FrameViewModel(frame);
                case Group group:
                    return new GroupViewModel(group);
                case MoveEffect moveEffect:
                    return new MoveEffectViewModel(moveEffect);
                case RotateEffect rotateEffect:
                    return new RotateEffectViewModel(rotateEffect);
                case ScaleEffect scaleEffect:
                    return new ScaleEffectViewModel(scaleEffect);
                case ShearEffect shearEffect:
                    return new ShearEffectViewModel(shearEffect);
                default:
                    throw new Exception(); // ToDo
            }
        }

        [Category("Group")]
        [PropertyOrder(1)]
        public ColorBlendMode ColorBlendMode
        {
            get => Group.ColorBlendMode;
            set
            {
                Group.ColorBlendMode = value;
                RaisePropertyChanged(nameof(ColorBlendMode));
            }
        }
    }
}
