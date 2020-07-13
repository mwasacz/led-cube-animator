using GalaSoft.MvvmLight.Command;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
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
        public GroupViewModel(Group group, IModelManager model) : base(group, model) { }

        public Group Group => (Group)Effect;

        private RelayCommand _editChildren;

        [Category("Group")]
        [DisplayName("Children")]
        [PropertyOrder(0)]
        public ICommand ChildrenProperty => _editChildren ?? (_editChildren = new RelayCommand(() => EditChildren?.Invoke(this, EventArgs.Empty))); // ToDo

        public event EventHandler EditChildren;

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
