using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.ViewModel
{
    public class GroupViewModel : EffectViewModel
    {
        public GroupViewModel(Group group) : base(group)
        {
            Children = new ObservableCollection<TileViewModel>(group.Children.Select(CreateViewModel));

            Children.CollectionChanged += Children_CollectionChanged;
        }

        public Group Group => (Group)Effect;

        public ObservableCollection<TileViewModel> Children { get; }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Group.Children.Clear();
            Group.Children.AddRange(Children.Select(c => c.Tile));
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
