using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LedCubeAnimator.Model;
using System.Windows.Input;

namespace LedCubeAnimator.ViewModel.UserControlViewModels
{
    public class PropertyViewModel : ViewModelBase
    {
        public PropertyViewModel(IModelManager model, ISharedViewModel shared)
        {
            Model = model;
            Shared = shared;
        }

        public IModelManager Model { get; }
        public ISharedViewModel Shared { get; }

        private RelayCommand _removeTileCommand;
        public ICommand RemoveTileCommand => _removeTileCommand ?? (_removeTileCommand = new RelayCommand(() =>
        {
            if (Shared.SelectedTile.Parent != null)
            {
                Model.RemoveTile(Shared.SelectedTile.Parent.Group, Shared.SelectedTile.Tile);
            }
        }));
    }
}
