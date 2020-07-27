using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.Collections.Generic;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Group", 2)]
    public class GroupViewModel : EffectViewModel
    {
        public GroupViewModel(Group group, IModelManager model) : base(group, model) { }

        public Group Group => (Group)Effect;

        [Category("Group")]
        [DisplayName("Children")]
        [PropertyOrder(0)]
        public object ChildrenProperty { get; }

        public List<Tile> Children => Group.Children;

        [Category("Group")]
        [PropertyOrder(1)]
        public ColorBlendMode ColorBlendMode
        {
            get => Group.ColorBlendMode;
            set => Model.SetTileProperty(Group, nameof(Group.ColorBlendMode), value);
        }

        public override void ModelPropertyChanged(string propertyName)
        {
            base.ModelPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Group.Children):
                    RaisePropertyChanged(nameof(Children));
                    break;
                case nameof(Group.ColorBlendMode):
                    RaisePropertyChanged(nameof(ColorBlendMode));
                    break;
            }
        }
    }
}
