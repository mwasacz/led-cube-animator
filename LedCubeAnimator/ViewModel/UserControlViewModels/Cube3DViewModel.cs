using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Utils;
using LedCubeAnimator.ViewModel.DataViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace LedCubeAnimator.ViewModel.UserControlViewModels
{
    public class Cube3DViewModel : ViewModelBase
    {
        public Cube3DViewModel(IModelManager model, ISharedViewModel shared)
        {
            Model = model;
            Shared = shared;

            Model.AnimationChanged += Model_AnimationChanged;
            Model.PropertiesChanged += Model_PropertiesChanged;
            Shared.PropertyChanged += Shared_PropertyChanged;

            RenderFrame();
        }

        public IModelManager Model { get; }
        public ISharedViewModel Shared { get; }

        private Color[,,] _frame;
        public Color[,,] Frame
        {
            get => _frame;
            private set => Set(ref _frame, value);
        }

        private RelayCommand<Point3D> _voxelClickCommand;
        public ICommand VoxelClickCommand => _voxelClickCommand ?? (_voxelClickCommand = new RelayCommand<Point3D>(p =>
        {
            if (Shared.SelectedTiles.Count == 1 && Shared.SelectedTiles[0] is FrameViewModel frame)
            {
                int x = (int)p.X - (int)frame.Offset.X;
                int y = (int)p.Y - (int)frame.Offset.Y;
                int z = (int)p.Z - (int)frame.Offset.Z;
                var voxels = frame.Frame.Voxels;

                if (x >= 0 && y >= 0 && z >= 0 && x < voxels.GetLength(0) && y < voxels.GetLength(1) && z < voxels.GetLength(2))
                {
                    if (Shared.SelectedColor.HasValue)
                    {
                        if (Model.Animation.ColorMode == ColorMode.Mono)
                        {
                            var color = voxels[x, y, z].GetBrightness() > 127 ? Colors.Black : Colors.White;
                            Model.SetVoxel(frame.Frame, color, x, y, z);
                        }
                        else
                        {
                            Model.SetVoxel(frame.Frame, Shared.SelectedColor.Value, x, y, z);
                        }
                    }
                    Shared.ColorClick(voxels[x, y, z]);
                }
            }
        }));

        private void RenderFrame()
        {
            Frame = Renderer.Render(Model.Animation, Shared.CurrentGroup.Group, Shared.Time, true);
        }

        private void Model_AnimationChanged(object sender, EventArgs e)
        {
            RenderFrame();
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            if (e.Changes.Any(c => c.Value != nameof(Tile.Name) && !(c.Key is Animation && c.Value == nameof(Animation.FrameDuration))))
            {
                RenderFrame();
            }
        }

        private void Shared_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISharedViewModel.Time) || e.PropertyName == nameof(ISharedViewModel.CurrentGroup))
            {
                RenderFrame();
            }
        }
    }
}
