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
    public abstract class TileViewModel : BaseViewModel, IDisposable
    {
        public TileViewModel(Tile tile, UndoManager undo) : base(undo)
        {
            Tile = tile;
            Undo.ActionExecuted += Undo_ActionExecuted;
        }

        public Tile Tile { get; }

        [Category("Tile")]
        [PropertyOrder(0)]
        public string Name
        {
            get => Tile.Name;
            set => Set(Tile, nameof(Tile.Name), value);
        }

        [Category("Tile")]
        [PropertyOrder(1)]
        public int Start
        {
            get => Tile.Start;
            set => Set(Tile, nameof(Tile.Start), value);
        }

        [Category("Tile")]
        [PropertyOrder(2)]
        public int End
        {
            get => Tile.End;
            set => Set(Tile, nameof(Tile.End), value);
        }

        [Category("Tile")]
        [PropertyOrder(3)]
        public int Hierarchy
        {
            get => Tile.Hierarchy;
            set => Set(Tile, nameof(Tile.Hierarchy), value);
        }

        private void Undo_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            if (e.Action is PropertyChangeAction action && action.Object == Tile)
            {
                ModelPropertyChanged(action.Property);
            }
        }

        protected virtual void ModelPropertyChanged(string propertyName)
        {
            switch (propertyName)
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

        public void Dispose()
        {
            Undo.ActionExecuted -= Undo_ActionExecuted;
        }
    }
}
