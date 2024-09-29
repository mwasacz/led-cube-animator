using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    public abstract class TileViewModel : ViewModelBase
    {
        public TileViewModel(Tile tile, IModelManager model, IMessenger messenger, GroupViewModel parent) : base(messenger)
        {
            Tile = tile;
            Model = model;
            Parent = parent;

            Model.PropertiesChanged += Model_PropertiesChanged;

            UpdateLength();
        }

        private int _length;
        private int _row;
        private bool _selected;

        [Browsable(false)]
        public Tile Tile { get; }

        [Browsable(false)]
        public IModelManager Model { get; }

        [Browsable(false)]
        public GroupViewModel Parent { get; }

        [Category("Tile")]
        [PropertyOrder(0)]
        public string Name
        {
            get => Tile.Name;
            set => Model.SetTileProperty(Tile, nameof(Tile.Name), value);
        }

        [Category("Tile")]
        [PropertyOrder(1)]
        [Range(0, int.MaxValue)]
        public int Start
        {
            get => Tile.Start;
            set => Model.SetTileProperty(Tile, nameof(Tile.Start), value);
        }

        [Category("Tile")]
        [PropertyOrder(2)]
        [Range(0, int.MaxValue)]
        public int End
        {
            get => Tile.End;
            set => Model.SetTileProperty(Tile, nameof(Tile.End), value);
        }

        [Category("Tile")]
        [PropertyOrder(3)]
        [Range(0, int.MaxValue)]
        public int Channel
        {
            get => Tile.Channel;
            set => Model.SetTileProperty(Tile, nameof(Tile.Channel), value);
        }

        [Category("Tile")]
        [PropertyOrder(4)]
        [Range(0, int.MaxValue)]
        public int Hierarchy
        {
            get => Tile.Hierarchy;
            set => Model.SetTileProperty(Tile, nameof(Tile.Hierarchy), value);
        }

        [Browsable(false)]
        public int Length
        {
            get => _length;
            private set => Set(ref _length, value);
        }

        [Browsable(false)]
        public int Row
        {
            get => _row;
            set => Set(ref _row, value);
        }

        [Browsable(false)]
        public bool Selected
        {
            get => _selected;
            set => Set(ref _selected, value, true);
        }

        [Browsable(false)]
        public new bool IsInDesignMode => base.IsInDesignMode;

        public override void Cleanup()
        {
            Selected = false;
            Model.PropertiesChanged -= Model_PropertiesChanged;
            base.Cleanup();
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            foreach (var change in e.Changes)
            {
                if (change.Key == Tile)
                {
                    ModelPropertyChanged(change.Value);
                }
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
                    UpdateLength();
                    break;
                case nameof(Tile.End):
                    RaisePropertyChanged(nameof(End));
                    UpdateLength();
                    break;
                case nameof(Tile.Channel):
                    RaisePropertyChanged(nameof(Channel));
                    break;
                case nameof(Tile.Hierarchy):
                    RaisePropertyChanged(nameof(Hierarchy));
                    break;
            }
        }

        private void UpdateLength()
        {
            Length = End - Start + 1;
        }

        protected Point GetNewValue(Point newValue, Point oldValue)
        {
            return new Point(
                double.IsNaN(newValue.X) ? oldValue.X : newValue.X,
                double.IsNaN(newValue.Y) ? oldValue.Y : newValue.Y);
        }

        protected Vector GetNewValue(Vector newValue, Vector oldValue)
        {
            return new Vector(
                double.IsNaN(newValue.X) ? oldValue.X : newValue.X,
                double.IsNaN(newValue.Y) ? oldValue.Y : newValue.Y);
        }

        protected Point3D GetNewValue(Point3D newValue, Point3D oldValue)
        {
            return new Point3D(
                double.IsNaN(newValue.X) ? oldValue.X : newValue.X,
                double.IsNaN(newValue.Y) ? oldValue.Y : newValue.Y,
                double.IsNaN(newValue.Z) ? oldValue.Z : newValue.Z);
        }

        protected Vector3D GetNewValue(Vector3D newValue, Vector3D oldValue)
        {
            return new Vector3D(
                double.IsNaN(newValue.X) ? oldValue.X : newValue.X,
                double.IsNaN(newValue.Y) ? oldValue.Y : newValue.Y,
                double.IsNaN(newValue.Z) ? oldValue.Z : newValue.Z);
        }
    }
}
