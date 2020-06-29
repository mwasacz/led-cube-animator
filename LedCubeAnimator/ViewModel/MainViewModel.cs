﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LedCubeAnimator.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ColorItem = Xceed.Wpf.Toolkit.ColorItem;

namespace LedCubeAnimator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            CreateDefaultAnimation();
        }

        private void CreateDefaultAnimation()
        {
            var group = new Group { Name = "MainGroup" };
            _animation = new Animation { MainGroup = group, Size = 4, MonoColor = Colors.White };
            CreateDefaultViewModel();
        }

        private void CreateDefaultViewModel()
        {
            var group = new GroupViewModel(_animation.MainGroup);
            Groups.Clear();
            Groups.Add(group);
            SelectedGroup = group;

            RaisePropertyChanged(nameof(ColorMode)); // ToDo: raise only if changed

            SelectedColor = Colors.Black;
            _recentColors.Clear();
            RecentColors.Clear();
            AddColorToRecents(Colors.Black);
            ColorPickerTool = false;

            RenderFrame();
        }

        private Animation _animation;

        public ObservableCollection<TileViewModel> Tiles => Groups.Last().Children;

        public ObservableCollection<GroupViewModel> Groups { get; } = new ObservableCollection<GroupViewModel>();

        private TileViewModel _selectedTile;
        public TileViewModel SelectedTile
        {
            get => _selectedTile;
            set
            {
                _selectedTile = value;
                if (_selectedGroup != null)
                {
                    _selectedGroup.PropertyChanged -= SelectedGroup_PropertyChanged;
                }
                _selectedGroup = null;

                RaisePropertyChanged(nameof(SelectedTile));
                RaisePropertyChanged(nameof(SelectedGroup));
                RaisePropertyChanged(nameof(SelectedTileOrGroup));
            }
        }

        private GroupViewModel _selectedGroup;
        public GroupViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedTile = null;
                if (_selectedGroup != null)
                {
                    _selectedGroup.PropertyChanged -= SelectedGroup_PropertyChanged;
                }
                _selectedGroup = value;
                if (_selectedGroup != null)
                {
                    _selectedGroup.PropertyChanged += SelectedGroup_PropertyChanged;
                }

                RaisePropertyChanged(nameof(SelectedTile));
                RaisePropertyChanged(nameof(SelectedGroup));
                RaisePropertyChanged(nameof(SelectedTileOrGroup));

                if (_selectedGroup != null)
                {
                    while (_selectedGroup != Groups.Last())
                    {
                        Groups.RemoveAt(Groups.Count - 1);
                    }
                    RaisePropertyChanged(nameof(Tiles));
                    RaisePropertyChanged(nameof(EndDate));
                    ClampTime();
                }
            }
        }

        public TileViewModel SelectedTileOrGroup => SelectedTile ?? SelectedGroup;

        private void SelectedGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GroupViewModel.Start)
                || e.PropertyName == nameof(GroupViewModel.End)
                || e.PropertyName == nameof(GroupViewModel.RepeatCount)
                || e.PropertyName == nameof(GroupViewModel.Reverse))
            {
                RaisePropertyChanged(nameof(EndDate));
                ClampTime();
            }
        }

        private string _selectedProperty;
        public string SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                _selectedProperty = value;
                if (_selectedProperty == "Children" && SelectedTile is GroupViewModel group)
                {
                    Groups.Add(group);
                    SelectedGroup = group;
                }
            }
        }

        // ToDo: consider storing brightness as alpha
        public ColorMode ColorMode => _animation.ColorMode;

        private Color _selectedColor;
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                _recentColor = _recentColors.SingleOrDefault(p => p.Item2 == _selectedColor)?.Item1;
                RaisePropertyChanged(nameof(SelectedColor));
                RaisePropertyChanged(nameof(DisplayColor));
                RaisePropertyChanged(nameof(RecentColor));
            }
        }

        public Color DisplayColor => SelectedColor.Multiply(_animation.MonoColor);

        private ColorItem _recentColor;
        public ColorItem RecentColor
        {
            get => _recentColor;
            set
            {
                _recentColor = value;
                if (_recentColor != null)
                {
                    _selectedColor = _recentColors.Single(p => p.Item1 == _recentColor).Item2;
                }
                RaisePropertyChanged(nameof(SelectedColor));
                RaisePropertyChanged(nameof(DisplayColor));
                RaisePropertyChanged(nameof(RecentColor));
            }
        }

        // ToDo: create type containing ColorItem and Color and use converters to display RecentColor and RecentColors
        private List<Tuple<ColorItem, Color>> _recentColors = new List<Tuple<ColorItem, Color>>();
        public ObservableCollection<ColorItem> RecentColors { get; } = new ObservableCollection<ColorItem>();

        private bool _colorPickerTool;
        public bool ColorPickerTool
        {
            get => _colorPickerTool;
            set
            {
                _colorPickerTool = value;
                RaisePropertyChanged(nameof(ColorPickerTool));
            }
        }

        public int StartDate => 0;

        public int EndDate
        {
            get
            {
                var group = Groups.Last();
                int end = (group.End - group.Start) / group.RepeatCount;
                if (group.Reverse)
                {
                    end /= 2;
                }
                return end;
            }
        }

        private RelayCommand _addFrameCommand;
        public ICommand AddFrameCommand => _addFrameCommand ?? (_addFrameCommand = new RelayCommand(() =>
        {
            var frame = new FrameViewModel(new Frame { Name = "Frame" });
            frame.To = new Point3D(_animation.Size - 1, _animation.Size - 1, _animation.Size - 1);
            Tiles.Add(frame);
            SelectedTile = frame;
        }));

        private RelayCommand _addGroupCommand;
        public ICommand AddGroupCommand => _addGroupCommand ?? (_addGroupCommand = new RelayCommand(() =>
        {
            var group = new GroupViewModel(new Group { Name = "Group" });
            Tiles.Add(group);
            SelectedTile = group;
        }));

        private RelayCommand _addMoveEffectCommand;
        public ICommand AddMoveEffectCommand => _addMoveEffectCommand ?? (_addMoveEffectCommand = new RelayCommand(() =>
        {
            var effect = new MoveEffectViewModel(new MoveEffect { Name = "MoveEffect" });
            Tiles.Add(effect);
            SelectedTile = effect;
        }));

        private RelayCommand _addRotateEffectCommand;
        public ICommand AddRotateEffectCommand => _addRotateEffectCommand ?? (_addRotateEffectCommand = new RelayCommand(() =>
        {
            var effect = new RotateEffectViewModel(new RotateEffect { Name = "RotateEffect" });
            Tiles.Add(effect);
            SelectedTile = effect;
        }));

        private RelayCommand _addScaleEffectCommand;
        public ICommand AddScaleEffectCommand => _addScaleEffectCommand ?? (_addScaleEffectCommand = new RelayCommand(() =>
        {
            var effect = new ScaleEffectViewModel(new ScaleEffect { Name = "ScaleEffect" });
            Tiles.Add(effect);
            SelectedTile = effect;
        }));

        private RelayCommand _addShearEffectCommand;
        public ICommand AddShearEffectCommand => _addShearEffectCommand ?? (_addShearEffectCommand = new RelayCommand(() =>
        {
            var effect = new ShearEffectViewModel(new ShearEffect { Name = "ShearEffect" });
            Tiles.Add(effect);
            SelectedTile = effect;
        }));

        private RelayCommand _removeTileCommand;
        public ICommand RemoveTileCommand => _removeTileCommand ?? (_removeTileCommand = new RelayCommand(() =>
        {
            if (SelectedTile != null)
            {
                Tiles.Remove(SelectedTile);
                SelectedGroup = Groups.Last();
            }
            else if (SelectedGroup != null && Groups.Count > 1)
            {
                SelectedGroup = Groups[Groups.Count - 2];
            }
        }));

        private RelayCommand _cubeSettingsCommand;
        public ICommand CubeSettingsCommand => _cubeSettingsCommand ?? (_cubeSettingsCommand = new RelayCommand(() =>
        {
            var viewModel = new CubeSettingsViewModel
            {
                Size = _animation.Size,
                ColorMode = _animation.ColorMode,
                MonoColor = _animation.MonoColor,
                FrameDuration = _animation.FrameDuration
            };
            var dialog = new CubeSettingsDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                _animation.Size = viewModel.Size;
                _animation.ColorMode = viewModel.ColorMode;
                _animation.MonoColor = viewModel.MonoColor;
                _animation.FrameDuration = viewModel.FrameDuration;

                RaisePropertyChanged(nameof(ColorMode));
                RaisePropertyChanged(nameof(DisplayColor));
                if (ColorMode == ColorMode.Mono)
                {
                    ColorPickerTool = false;
                }
                RenderFrame();
            }
        }));

        private Color[,,] _frame;
        public Color[,,] Frame
        {
            get => _frame;
            set
            {
                _frame = value;
                RaisePropertyChanged(nameof(Frame));
            }
        }

        private RelayCommand<Point3D> _voxelClickCommand;
        public ICommand VoxelClickCommand => _voxelClickCommand ?? (_voxelClickCommand = new RelayCommand<Point3D>(p =>
        {
            if (SelectedTile is FrameViewModel frame)
            {
                int y = (int)frame.To.Y - (int)frame.From.Y + 1;
                int z = (int)frame.To.Z - (int)frame.From.Z + 1;
                int index = (int)p.X * y * z + (int)p.Y * z + (int)p.Z;

                if (ColorPickerTool)
                {
                    SelectedColor = frame.Voxels[index];//.Opaque();
                    ColorPickerTool = false;
                }
                else
                {
                    if (_animation.ColorMode == ColorMode.Mono)
                    {
                        frame.Voxels[index] = frame.Voxels[index] == Colors.Black ? Colors.White : Colors.Black;
                    }
                    else
                    {
                        frame.Voxels[index] = SelectedColor;
                        AddColorToRecents(SelectedColor);
                    }
                    RenderFrame();
                }
            }
        }));

        private int _time;
        public int Time
        {
            get => _time;
            set
            {
                _time = value;
                ClampTime();
            }
        }

        private void ClampTime()
        {
            _time = Math.Min(Math.Max(StartDate, _time), EndDate);
            RaisePropertyChanged(nameof(Time));
            RenderFrame();
        }

        private void RenderFrame()
        {
            Frame = Renderer.Render(_animation, Time, true);
        }

        private void AddColorToRecents(Color color)
        {
            var pair = _recentColors.SingleOrDefault(p => p.Item2 == color);
            if (pair != null)
            {
                int index = RecentColors.IndexOf(pair.Item1);
                if (index != 0)
                {
                    RecentColors.Move(index, 0);
                }
                RecentColor = RecentColors[0];
            }
            else
            {
                var colorItem = ColorMode == ColorMode.RGB ?
                    new ColorItem(color, color.ToString()) :
                    new ColorItem(color.Multiply(_animation.MonoColor), color.GetBrightness().ToString());

                _recentColors.Add(new Tuple<ColorItem, Color>(colorItem, color));
                RecentColors.Insert(0, colorItem);
                RecentColor = RecentColors[0];

                if (RecentColors.Count > 10)
                {
                    _recentColors.RemoveAll(p => p.Item1 == RecentColors[10]);
                    RecentColors.RemoveAt(10);
                }
            }
        }

        private string _filePath;

        private RelayCommand _newCommand;
        public ICommand NewCommand => _newCommand ?? (_newCommand = new RelayCommand(() =>
        {
            switch (MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:
                    SaveCommand.Execute(null);
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    return;
            }

            _filePath = null;

            CreateDefaultAnimation();
        }));

        private RelayCommand _openCommand;
        public ICommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(() =>
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Animation files (*.json)|*.json|All files (*.*)|*.*";
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var a = FileReaderWriter.Open(dialog.FileName);

            if (a == null)
            {
                MessageBox.Show("Error occured when opening file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _filePath = dialog.FileName;

            _animation = a;
            CreateDefaultViewModel();
        }));

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() =>
        {
            if (_filePath != null)
            {
                FileReaderWriter.Save(_filePath, _animation);
            }
            else
            {
                SaveAsCommand.Execute(null);
            }
        }));

        private RelayCommand _saveAsCommand;
        public ICommand SaveAsCommand => _saveAsCommand ?? (_saveAsCommand = new RelayCommand(() =>
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Animation files (*.json)|*.json|All files (*.*)|*.*";
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            _filePath = dialog.FileName;

            FileReaderWriter.Save(_filePath, _animation);
        }));

        private RelayCommand _exportMWCommand;
        public ICommand ExportMWCommand => _exportMWCommand ?? (_exportMWCommand = new RelayCommand(() =>
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            Exporter.Export(dialog.FileName, _animation);
        }));
    }
}
