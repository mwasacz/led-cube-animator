using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedCubeAnimator.ViewModel
{
    public abstract class TileViewModel : ViewModelBase
    {
        public TileViewModel(Tile tile)
        {
            Tile = tile;
        }

        public Tile Tile { get; }

        public string Name
        {
            get => Tile.Name;
            set
            {
                Tile.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public int Start
        {
            get => Tile.Start;//new DateTime(Tile.Start + 1);
            set
            {
                Tile.Start = value;//(int)value.Ticks - 1;
                RaisePropertyChanged(nameof(Start));
            }
        }

        public int End
        {
            get => Tile.End;//new DateTime(Tile.Start + Tile.Duration + 1);
            set
            {
                Tile.End = value;//(int)value.Ticks - Tile.Start - 1;
                RaisePropertyChanged(nameof(End));
            }
        }

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
