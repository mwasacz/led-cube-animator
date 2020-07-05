using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Undo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel
{
    [CategoryOrder("Tile", 0)]
    public abstract class TileViewModel : ViewModelBase
    {
        public TileViewModel(Tile tile, UndoManager undo)
        {
            Tile = tile;
            Undo = undo;
        }

        public Tile Tile { get; }

        public UndoManager Undo { get; }

        [Category("Tile")]
        [PropertyOrder(0)]
        public string Name
        {
            get => Tile.Name;
            set => Undo.Set(Tile, nameof(Tile.Name), value);
        }

        [Category("Tile")]
        [PropertyOrder(1)]
        public int Start
        {
            get => Tile.Start;
            set => Undo.Set(Tile, nameof(Tile.Start), value);
        }

        [Category("Tile")]
        [PropertyOrder(2)]
        public int End
        {
            get => Tile.End;
            set => Undo.Set(Tile, nameof(Tile.End), value);
        }

        [Category("Tile")]
        [PropertyOrder(3)]
        public int Hierarchy
        {
            get => Tile.Hierarchy;
            set => Undo.Set(Tile, nameof(Tile.Hierarchy), value);
        }

        public virtual void ActionExecuted(IAction action)
        {
            if (action is PropertyChangeAction propertyAction && propertyAction.Object == Tile)
            {
                switch (propertyAction.Property)
                {
                    case nameof(Tile.Name):
                        RaisePropertyChanged(nameof(Name));
                        break;
                    case nameof(Tile.Start):
                        RaisePropertyChanged(nameof(Start));
                        break;
                    case nameof(Tile.End):
                        RaisePropertyChanged(nameof(End));
                        break;
                    case nameof(Tile.Hierarchy):
                        RaisePropertyChanged(nameof(Hierarchy));
                        break;
                }
            }
        }
    }
}
