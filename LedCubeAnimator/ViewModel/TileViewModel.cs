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
        public TileViewModel(Tile tile, IModelManager model)
        {
            Tile = tile;
            Model = model;
        }

        public Tile Tile { get; }

        public IModelManager Model { get; }

        [Category("Tile")]
        [PropertyOrder(0)]
        public string Name
        {
            get => Tile.Name;
            set => Model.SetTileProperty(Tile, nameof(Tile.Name), value);
        }

        [Category("Tile")]
        [PropertyOrder(1)]
        public int Start
        {
            get => Tile.Start;
            set => Model.SetTileProperty(Tile, nameof(Tile.Start), value);
        }

        [Category("Tile")]
        [PropertyOrder(2)]
        public int End
        {
            get => Tile.End;
            set => Model.SetTileProperty(Tile, nameof(Tile.End), value);
        }

        [Category("Tile")]
        [PropertyOrder(3)]
        public int Hierarchy
        {
            get => Tile.Hierarchy;
            set => Model.SetTileProperty(Tile, nameof(Tile.Hierarchy), value);
        }

        public virtual void ModelPropertyChanged(string propertyName)
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
    }
}
