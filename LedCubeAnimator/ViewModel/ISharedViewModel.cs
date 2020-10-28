using LedCubeAnimator.ViewModel.DataViewModels;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel
{
    public interface ISharedViewModel : INotifyPropertyChanged
    {
        TileViewModel SelectedTile { get; set; }
        bool SelectedTileExpanded { get; set; }
        string SelectedProperty { get; set; }

        Color? SelectedColor { get; set; }
        void ColorClick(Color color);
        event EventHandler<ColorClickedEventArgs> ColorClicked;

        int Time { get; set; }
        int Length { get; }
    }
}
