using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LedCubeAnimator.Model;
using Microsoft.Win32;
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
            Model = new ModelManager();
            CreateDefaultViewModel();
            Model.PropertiesChanged += Model_PropertiesChanged;
        }

        public IModelManager Model { get; }

        public Animation Animation => Model.Animation;

        private void CreateDefaultViewModel()
        {
            SelectedGroup = (GroupViewModel)CreateViewModel(Animation.MainGroup);

            RaisePropertyChanged(nameof(ColorMode)); // ToDo: raise only if changed

            ResetColors();

            Time = 0;

            _undoCommand?.RaiseCanExecuteChanged();
            _redoCommand?.RaiseCanExecuteChanged();
        }

        public ObservableCollection<TileViewModel> Tiles { get; } = new ObservableCollection<TileViewModel>();

        public ObservableCollection<GroupViewModel> Groups { get; } = new ObservableCollection<GroupViewModel>();

        private TileViewModel _selectedTile;
        public TileViewModel SelectedTile
        {
            get => _selectedTile;
            set
            {
                if (_selectedTile != value)
                {
                    IList<Group> parents = null;
                    if (value != null && !Tiles.Contains(value))
                    {
                        parents = FindTileParents(value.Tile);
                        AddNewTilesAndGroups(parents, value);
                    }

                    var oldTileOrGroup = _selectedTile ?? _selectedGroup;
                    _selectedTile = value;

                    Set(ref _selectedGroup, null, nameof(SelectedGroup));
                    RaisePropertyChanged(nameof(SelectedTile));
                    if (oldTileOrGroup != _selectedTile)
                    {
                        RaisePropertyChanged(nameof(SelectedTileOrGroup));
                    }

                    if (parents != null)
                    {
                        RemoveOldTilesAndGroups(parents);
                        ClampTime();
                    }
                }
            }
        }

        private GroupViewModel _selectedGroup;
        public GroupViewModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup != value)
                {
                    IList<Group> parents = null;
                    if (value != null)
                    {
                        parents = Groups.Contains(value)
                            ? Groups.TakeWhile(g => g != value).Append(value).Select(g => g.Group).ToArray()
                            : FindTileParents(value.Group).Append(value.Group).ToArray();
                        AddNewTilesAndGroups(parents, value);
                    }

                    var oldTileOrGroup = _selectedTile ?? _selectedGroup;
                    _selectedGroup = value;

                    Set(ref _selectedTile, null, nameof(SelectedTile));
                    RaisePropertyChanged(nameof(SelectedGroup));
                    if (oldTileOrGroup != _selectedGroup)
                    {
                        RaisePropertyChanged(nameof(SelectedTileOrGroup));
                    }

                    if (parents != null)
                    {
                        RemoveOldTilesAndGroups(parents);
                        ClampTime();
                    }
                }
            }
        }

        public TileViewModel SelectedTileOrGroup => SelectedTile ?? SelectedGroup;

        private TileViewModel CreateViewModel(Tile tile)
        {
            TileViewModel viewModel;
            switch (tile)
            {
                case Frame frame:
                    viewModel = new FrameViewModel(frame, Model);
                    break;
                case Group group:
                    viewModel = new GroupViewModel(group, Model);
                    break;
                case MoveEffect moveEffect:
                    viewModel = new MoveEffectViewModel(moveEffect, Model);
                    break;
                case RotateEffect rotateEffect:
                    viewModel = new RotateEffectViewModel(rotateEffect, Model);
                    break;
                case ScaleEffect scaleEffect:
                    viewModel = new ScaleEffectViewModel(scaleEffect, Model);
                    break;
                case ShearEffect shearEffect:
                    viewModel = new ShearEffectViewModel(shearEffect, Model);
                    break;
                case LinearDelayEffect linearDelayEffect:
                    viewModel = new LinearDelayEffectViewModel(linearDelayEffect, Model);
                    break;
                default:
                    throw new Exception(); // ToDo
            }
            viewModel.PropertyChanged += Tile_PropertyChanged;
            return viewModel;
        }

        private void DeleteViewModel(TileViewModel tile)
        {
            tile.PropertyChanged -= Tile_PropertyChanged;
        }

        private string _selectedProperty;
        public string SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                if (Set(ref _selectedProperty, value)
                    && SelectedTile is GroupViewModel group
                    && _selectedProperty == nameof(GroupViewModel.ChildrenProperty))
                {
                    SelectedGroup = group;
                }
            }
        }

        // ToDo: consider storing brightness as alpha
        public ColorMode ColorMode => Animation.ColorMode;

        private Color _selectedColor;
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (Set(ref _selectedColor, value))
                {
                    RaisePropertyChanged(nameof(DisplayColor));
                    Set(ref _recentColor, _recentColors.SingleOrDefault(p => p.Item2 == _selectedColor)?.Item1, nameof(RecentColor));
                }
            }
        }

        public Color DisplayColor => SelectedColor.Multiply(Animation.MonoColor);

        private ColorItem _recentColor;
        public ColorItem RecentColor
        {
            get => _recentColor;
            set
            {
                if (Set(ref _recentColor, value)
                    && _recentColor != null
                    && Set(ref _selectedColor, _recentColors.Single(p => p.Item1 == _recentColor).Item2, nameof(SelectedColor)))
                {
                    RaisePropertyChanged(nameof(DisplayColor));
                }
            }
        }

        // ToDo: create type containing ColorItem and Color and use converters to display RecentColor and RecentColors
        private List<Tuple<ColorItem, Color>> _recentColors = new List<Tuple<ColorItem, Color>>();
        public ObservableCollection<ColorItem> RecentColors { get; } = new ObservableCollection<ColorItem>();

        private bool _colorPickerTool;
        public bool ColorPickerTool
        {
            get => _colorPickerTool;
            set => Set(ref _colorPickerTool, value);
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
            AddTile(new Frame { Name = "Frame", Voxels = new Color[Animation.Size, Animation.Size, Animation.Size] })));

        private RelayCommand _addGroupCommand;
        public ICommand AddGroupCommand => _addGroupCommand ?? (_addGroupCommand = new RelayCommand(() => AddTile(new Group { Name = "Group" })));

        private RelayCommand _addMoveEffectCommand;
        public ICommand AddMoveEffectCommand => _addMoveEffectCommand ?? (_addMoveEffectCommand = new RelayCommand(() => AddTile(new MoveEffect { Name = "MoveEffect" })));

        private RelayCommand _addRotateEffectCommand;
        public ICommand AddRotateEffectCommand => _addRotateEffectCommand ?? (_addRotateEffectCommand = new RelayCommand(() => AddTile(new RotateEffect { Name = "RotateEffect" })));

        private RelayCommand _addScaleEffectCommand;
        public ICommand AddScaleEffectCommand => _addScaleEffectCommand ?? (_addScaleEffectCommand = new RelayCommand(() => AddTile(new ScaleEffect { Name = "ScaleEffect" })));

        private RelayCommand _addShearEffectCommand;
        public ICommand AddShearEffectCommand => _addShearEffectCommand ?? (_addShearEffectCommand = new RelayCommand(() => AddTile(new ShearEffect { Name = "ShearEffect" })));

        private RelayCommand _addLinearDelayEffectCommand;
        public ICommand AddLinearDelayEffectCommand => _addLinearDelayEffectCommand ?? (_addLinearDelayEffectCommand = new RelayCommand(() => AddTile(new LinearDelayEffect { Name = "LinearDelayEffect" })));

        private RelayCommand _removeTileCommand;
        public ICommand RemoveTileCommand => _removeTileCommand ?? (_removeTileCommand = new RelayCommand(() =>
        {
            if (SelectedTile != null)
            {
                Model.RemoveTile(Groups.Last().Group, SelectedTile.Tile);
            }
            else if (SelectedGroup != null && Groups.Count > 1)
            {
                Model.RemoveTile(Groups[Groups.Count - 2].Group, Groups.Last().Group);
            }
        }));

        private RelayCommand _cubeSettingsCommand;
        public ICommand CubeSettingsCommand => _cubeSettingsCommand ?? (_cubeSettingsCommand = new RelayCommand(() =>
        {
            var viewModel = new CubeSettingsViewModel
            {
                Size = Animation.Size,
                ColorMode = Animation.ColorMode,
                MonoColor = Animation.MonoColor,
                FrameDuration = Animation.FrameDuration
            };
            var dialog = new CubeSettingsDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                Model.SetAnimationProperties(viewModel.Size, viewModel.ColorMode, viewModel.MonoColor, viewModel.FrameDuration);
            }
        }));

        private Color[,,] _frame;
        public Color[,,] Frame
        {
            get => _frame;
            set => Set(ref _frame, value);
        }

        private RelayCommand<Point3D> _voxelClickCommand;
        public ICommand VoxelClickCommand => _voxelClickCommand ?? (_voxelClickCommand = new RelayCommand<Point3D>(p =>
        {
            if (SelectedTile is FrameViewModel frame)
            {
                int x = (int)p.X - (int)frame.Offset.X;
                int y = (int)p.Y - (int)frame.Offset.Y;
                int z = (int)p.Z - (int)frame.Offset.Z;

                if (x >= 0 && y >= 0 && z >= 0 && x < frame.Voxels.GetLength(0) && y < frame.Voxels.GetLength(1) && z < frame.Voxels.GetLength(2))
                {
                    if (ColorPickerTool)
                    {
                        SelectedColor = frame.Voxels[x, y, z];
                        ColorPickerTool = false;
                    }
                    else
                    {
                        if (Animation.ColorMode == ColorMode.Mono)
                        {
                            var color = frame.Voxels[x, y, z].GetBrightness() > 127 ? Colors.Black : Colors.White;
                            Model.SetVoxel(frame.Frame, color, x, y, z);
                        }
                        else
                        {
                            Model.SetVoxel(frame.Frame, SelectedColor, x, y, z);
                            AddColorToRecents(SelectedColor);
                        }
                    }
                }
            }
        }));

        private int _time;
        public int Time
        {
            get => _time;
            set
            {
                var time = Math.Max(StartDate, Math.Min(value, EndDate));
                if (Set(ref _time, time))
                {
                    RenderFrame();
                }
            }
        }

        private void ClampTime()
        {
            RaisePropertyChanged(nameof(EndDate)); // ToDo: raise if necessary
            if (Time > EndDate)
            {
                Time = EndDate;
            }
            else
            {
                RenderFrame();
            }
        }

        private void RenderFrame()
        {
            Frame = Renderer.Render(Animation, Time, true);
        }

        private void ResetColors()
        {
            SelectedColor = Colors.Black;
            _recentColors.Clear();
            RecentColors.Clear();
            AddColorToRecents(Colors.Black);
            ColorPickerTool = false;
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
                    new ColorItem(color.Multiply(Animation.MonoColor), color.GetBrightness().ToString());

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

        private void AddTile(Tile tile)
        {
            Model.AddTile(Groups.Last().Group, tile);
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            var animationChanges = e.Changes.Where(c => c.Key is Animation).ToArray();

            if (animationChanges.Length > 0)
            {
                if (animationChanges.Any(c => c.Value == nameof(Animation.ColorMode)))
                {
                    RaisePropertyChanged(nameof(ColorMode));
                }
                if (animationChanges.Any(c => c.Value == nameof(Animation.ColorMode) || c.Value == nameof(Animation.MonoColor)))
                {
                    ResetColors();
                }
                if (animationChanges.Any(c => c.Value == nameof(Animation.Size) || c.Value == nameof(Animation.ColorMode) || c.Value == nameof(Animation.MonoColor)))
                {
                    RenderFrame();
                }
            }
            else
            {
                var change = e.Changes.First(c => c.Key is Tile);
                var viewModel = Tiles.Concat(Groups).SingleOrDefault(t => t.Tile == change.Key) ?? CreateViewModel((Tile)change.Key);
                viewModel.ModelPropertyChanged(change.Value);
            }

            _undoCommand?.RaiseCanExecuteChanged(); // ToDo: use CommandWpf ???
            _redoCommand?.RaiseCanExecuteChanged();
        }

        private void Tile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var tile = (TileViewModel)sender;

            if (tile is GroupViewModel group && e.PropertyName == nameof(GroupViewModel.Children))
            {
                if (SelectedGroup == group)
                {
                    var groups = Groups.Select(g => g.Group).ToArray();
                    AddNewTilesAndGroups(groups, null);
                    RemoveOldTilesAndGroups(groups);
                }
                else
                {
                    SelectedGroup = group;
                }

                SelectedProperty = nameof(GroupViewModel.ChildrenProperty);

                RenderFrame();
            }
            else
            {
                if (Groups.Contains(tile))
                {
                    SelectedGroup = (GroupViewModel)tile;
                }
                else
                {
                    SelectedTile = tile;
                }

                SelectedProperty = tile is FrameViewModel && e.PropertyName == nameof(FrameViewModel.Voxels)
                    ? nameof(FrameViewModel.VoxelsProperty)
                    : e.PropertyName;

                if (tile is GroupViewModel && (e.PropertyName == nameof(GroupViewModel.Start)
                    || e.PropertyName == nameof(GroupViewModel.End)
                    || e.PropertyName == nameof(GroupViewModel.RepeatCount)
                    || e.PropertyName == nameof(GroupViewModel.Reverse)))
                {
                    ClampTime();
                }
                else
                {
                    RenderFrame();
                }
            }
        }

        private List<Group> FindTileParents(Tile tile)
        {
            var parents = new List<Group>();
            FindTileParents(tile, parents, Animation.MainGroup);
            parents.Reverse();
            return parents;
        }

        private static bool FindTileParents(Tile tile, List<Group> parents, Group group)
        {
            foreach (var t in group.Children)
            {
                if (t == tile || (t is Group g && FindTileParents(tile, parents, g)))
                {
                    parents.Add(group);
                    return true;
                }
            }
            return false;
        }

        private void AddNewTilesAndGroups(IList<Group> newGroups, TileViewModel newTile)
        {
            var newTiles = newGroups.Last().Children;

            foreach (var tile in newTiles)
            {
                if (!Tiles.Any(t => t.Tile == tile))
                {
                    Tiles.Add(Groups.Prepend(newTile).FirstOrDefault(t => t?.Tile == tile) ?? CreateViewModel(tile));
                }
            }
            foreach (var group in newGroups)
            {
                if (!Groups.Any(g => g.Group == group))
                {
                    Groups.Add((GroupViewModel)(Tiles.Prepend(newTile).FirstOrDefault(t => t?.Tile == group) ?? CreateViewModel(group)));
                }
            }
        }

        private void RemoveOldTilesAndGroups(IList<Group> newGroups)
        {
            var newTiles = newGroups.Last().Children;

            for (int i = Tiles.Count - 1; i >= 0; i--)
            {
                var tile = Tiles[i];
                if (!newTiles.Contains(tile.Tile))
                {
                    Tiles.Remove(tile);
                    if (!newGroups.Contains(tile.Tile))
                    {
                        DeleteViewModel(tile);
                    }
                }
            }
            for (int i = Groups.Count - 1; i >= 0; i--)
            {
                var group = Groups[i];
                if (!newGroups.Contains(group.Group))
                {
                    Groups.Remove(group);
                    if (!newTiles.Contains(group.Group))
                    {
                        DeleteViewModel(group);
                    }
                }
            }
        }

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

            Model.New();
            CreateDefaultViewModel();
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

            if (!Model.Open(dialog.FileName))
            {
                MessageBox.Show("Error occured when opening file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CreateDefaultViewModel();
        }));

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() =>
        {
            if (!Model.Save())
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

            Model.SaveAs(dialog.FileName);
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

            Model.Export(dialog.FileName);
        }));

        private RelayCommand _undoCommand;
        public ICommand UndoCommand => _undoCommand ?? (_undoCommand = new RelayCommand(() =>
        {
            if (Model.CanUndo)
            {
                Model.Undo();
            }
        }, () => Model.CanUndo));

        private RelayCommand _redoCommand;
        public ICommand RedoCommand => _redoCommand ?? (_redoCommand = new RelayCommand(() =>
        {
            if (Model.CanRedo)
            {
                Model.Redo();
            }
        }, () => Model.CanRedo));
    }
}
