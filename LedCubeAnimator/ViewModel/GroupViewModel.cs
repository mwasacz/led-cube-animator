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
        public GroupViewModel(Group group) : base(group) { }

        public Group Group => (Group)Effect;

        private RelayCommand _editChildren;

        [Category("Group")]
        [PropertyOrder(0)]
        public ICommand Children => _editChildren ?? (_editChildren = new RelayCommand(() => EditChildren?.Invoke(this, EventArgs.Empty))); // ToDo

        public event EventHandler EditChildren;

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
