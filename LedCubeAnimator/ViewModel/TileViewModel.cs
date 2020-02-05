using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
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
        public TileViewModel(Tile tile)
        {
            Tile = tile;
        }

        public Tile Tile { get; }

        [Category("Tile")]
        [PropertyOrder(0)]
        public string Name
        {
            get => Tile.Name;
            set
            {
                Tile.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        [Category("Tile")]
        [PropertyOrder(1)]
        public int Start
        {
            get => Tile.Start;//new DateTime(Tile.Start + 1);
            set
            {
                Tile.Start = value;//(int)value.Ticks - 1;
                RaisePropertyChanged(nameof(Start));
            }
        }

        [Category("Tile")]
        [PropertyOrder(2)]
        public int End
        {
            get => Tile.End;//new DateTime(Tile.Start + Tile.Duration + 1);
            set
            {
                Tile.End = value;//(int)value.Ticks - Tile.Start - 1;
                RaisePropertyChanged(nameof(End));
            }
        }

        [Category("Tile")]
        [PropertyOrder(3)]
        public int Hierarchy
        {
            get => Tile.Hierarchy;
            set
            {
                Tile.Hierarchy = value;
                RaisePropertyChanged(nameof(Hierarchy));
            }
        }
    }
}
