﻿using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace LedCubeAnimator.ViewModel.DataViewModels
{
    [CategoryOrder("Tile", 0)]
    public abstract class TileViewModel : ViewModelBase
    {
        public TileViewModel(Tile tile, IModelManager model, GroupViewModel parent)
        {
            Tile = tile;
            Model = model;
            Parent = parent;
        }

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
        public int Channel
        {
            get => Tile.Channel;
            set => Model.SetTileProperty(Tile, nameof(Tile.Channel), value);
        }

        [Category("Tile")]
        [PropertyOrder(4)]
        public int Hierarchy
        {
            get => Tile.Hierarchy;
            set => Model.SetTileProperty(Tile, nameof(Tile.Hierarchy), value);
        }

        [Browsable(false)]
        public new bool IsInDesignMode => base.IsInDesignMode;

        public virtual void ModelPropertyChanged(object obj, string propertyName, out TileViewModel changedViewModel, out string changedProperty)
        {
            changedProperty = null;
            changedViewModel = null;
            if (obj == Tile)
            {
                switch (propertyName)
                {
                    case nameof(Tile.Name):
                        changedProperty = nameof(Name);
                        break;
                    case nameof(Tile.Start):
                        changedProperty = nameof(Start);
                        break;
                    case nameof(Tile.End):
                        changedProperty = nameof(End);
                        break;
                    case nameof(Tile.Channel):
                        changedProperty = nameof(Channel);
                        break;
                    case nameof(Tile.Hierarchy):
                        changedProperty = nameof(Hierarchy);
                        break;
                    default:
                        return;
                }
                changedViewModel = this;
                RaisePropertyChanged(changedProperty);
            }
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
