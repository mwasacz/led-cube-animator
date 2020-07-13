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
            var group = (GroupViewModel)CreateViewModel(Animation.MainGroup);
            UpdateTilesAndGroups(new[] { group }, Animation.MainGroup.Children.Select(CreateViewModel), group);

            RaisePropertyChanged(nameof(ColorMode)); // ToDo: raise only if changed

            SelectedColor = Colors.Black;
            _recentColors.Clear();
            RecentColors.Clear();
            AddColorToRecents(Colors.Black);
            ColorPickerTool = false;

            RenderFrame();
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
                    if (_selectedTile is GroupViewModel g)
                    {
                        g.EditChildren -= Group_EditChildren;
                    }

                    _selectedTile = value;

                    if (_selectedTile is GroupViewModel group)
                    {
                        group.EditChildren += Group_EditChildren;
                    }

                    Set(ref _selectedGroup, null, nameof(SelectedGroup));
                    RaisePropertyChanged(nameof(SelectedTile));
                    RaisePropertyChanged(nameof(SelectedTileOrGroup));
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
                    if (_selectedTile is GroupViewModel group)
                    {
                        group.EditChildren -= Group_EditChildren;
                    }

                    _selectedGroup = value;
                    Set(ref _selectedTile, null, nameof(SelectedTile));
                    RaisePropertyChanged(nameof(SelectedGroup));
                    RaisePropertyChanged(nameof(SelectedTileOrGroup));

                    if (_selectedGroup != null && _selectedGroup != Groups.Last())
                    {
                        UpdateTilesAndGroups(Groups.Take(Groups.IndexOf(_selectedGroup) + 1),
                            _selectedGroup.Children.Select(FindTileViewModel));
                    }
                }
            }
        }

        public TileViewModel SelectedTileOrGroup => SelectedTile ?? SelectedGroup;

        private void Group_EditChildren(object sender, EventArgs e)
        {
            var group = (GroupViewModel)SelectedTile;
            UpdateTilesAndGroups(Groups.Append(group), group.Children.Select(CreateViewModel), group);
        }

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
            set => Set(ref _selectedProperty, value);
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
                Model.StartGroupChange();
                Model.SetAnimationProperty(nameof(Animation.Size), viewModel.Size);
                Model.SetAnimationProperty(nameof(Animation.ColorMode), viewModel.ColorMode);
                Model.SetAnimationProperty(nameof(Animation.MonoColor), viewModel.MonoColor);
                Model.SetAnimationProperty(nameof(Animation.FrameDuration), viewModel.FrameDuration);
                Model.EndGroupChange();
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
            if (SelectedTile is FrameViewModel frameViewModel)
            {
                var frame = frameViewModel.Frame;
                int x = (int)p.X;
                int y = (int)p.Y;
                int z = (int)p.Z;

                if (ColorPickerTool)
                {
                    SelectedColor = frame.Voxels[x, y, z];//.Opaque();
                    ColorPickerTool = false;
                }
                else
                {
                    if (Animation.ColorMode == ColorMode.Mono)
                    {
                        var color = frame.Voxels[x, y, z] == Colors.Black ? Colors.White : Colors.Black;
                        Model.SetVoxel(frame, color, x, y, z);
                    }
                    else
                    {
                        Model.SetVoxel(frame, SelectedColor, x, y, z);
                        AddColorToRecents(SelectedColor);
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
                if (Set(ref _time, value))
                {
                    RenderFrame();
                }
            }
        }

        private void RenderFrame()
        {
            Frame = Renderer.Render(Animation, Time, true);
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
            var change = e.Changes.LastOrDefault(c => c.Object is Tile);

            if (change != null)
            {
                var viewModel = Tiles.Concat(Groups).SingleOrDefault(t => t.Tile == change.Object) ?? CreateViewModel((Tile)change.Object);
                viewModel.ModelPropertyChanged(change.Property);
            }

            _undoCommand.RaiseCanExecuteChanged(); // ToDo: use CommandWpf ???
            _redoCommand.RaiseCanExecuteChanged();
        }

        private void Tile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var tile = (TileViewModel)sender;

            if (tile is GroupViewModel group && e.PropertyName == nameof(GroupViewModel.Children))
            {
                NavigateToGroup(group);

                SelectedProperty = nameof(GroupViewModel.ChildrenProperty);
            }
            else
            {
                NavigateToTile(tile);

                SelectedProperty = tile is FrameViewModel && e.PropertyName == nameof(FrameViewModel.Voxels)
                    ? nameof(FrameViewModel.VoxelsProperty)
                    : e.PropertyName;
            }

            RenderFrame(); // ToDo: render if necessary
        }

        private void NavigateToTile(TileViewModel tile)
        {
            if (Tiles.Contains(tile))
            {
                SelectedTile = tile;
            }
            else if (tile is GroupViewModel group && Groups.Contains(group))
            {
                SelectedGroup = group;
            }
            else
            {
                var parents = new List<Group>();
                FindTileParents(tile.Tile, parents, Animation.MainGroup);
                parents.Reverse();
                UpdateTilesAndGroups(parents.Select(FindGroupViewModel),
                    parents.Last().Children.Select(t => t == tile.Tile ? tile : FindTileViewModel(t)),
                    tile);
            }
        }

        private void NavigateToGroup(GroupViewModel group)
        {
            if (Groups.Last() == group)
            {
                SelectedGroup = group;
                UpdateTilesAndGroups(Groups, group.Children.Select(FindTileViewModel));
            }
            else if (Groups.Contains(group))
            {
                SelectedGroup = group;
            }
            else
            {
                var parents = new List<Group>();
                FindTileParents(group.Group, parents, Animation.MainGroup);
                parents.Reverse();
                UpdateTilesAndGroups(parents.Select(FindGroupViewModel).Append(group),
                    group.Children.Select(FindTileViewModel),
                    group);
            }
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

        private TileViewModel FindTileViewModel(Tile tile) => Tiles.Concat(Groups).SingleOrDefault(t => t.Tile == tile) ?? CreateViewModel(tile);

        private GroupViewModel FindGroupViewModel(Group group) => (GroupViewModel)FindTileViewModel(group);

        private void UpdateTilesAndGroups(IEnumerable<GroupViewModel> groups, IEnumerable<TileViewModel> tiles, TileViewModel selectedTile = null)
        {
            var newTiles = tiles.ToArray();
            var newGroups = groups.ToArray();

            foreach (var tile in newTiles.Except(Tiles).ToArray())
            {
                Tiles.Add(tile);
            }
            foreach (var group in newGroups.Except(Groups).ToArray())
            {
                Groups.Add(group);
            }

            if (newTiles.Contains(selectedTile))
            {
                SelectedTile = selectedTile;
            }
            else if (newGroups.Contains(selectedTile))
            {
                SelectedGroup = (GroupViewModel)selectedTile;
            }

            foreach (var tile in Tiles.Except(newTiles).ToArray())
            {
                Tiles.Remove(tile);
                if (!Groups.Contains(tile))
                {
                    DeleteViewModel(tile);
                }
            }
            foreach (var group in Groups.Except(newGroups).ToArray())
            {
                Groups.Remove(group);
                if (!Tiles.Contains(group))
                {
                    DeleteViewModel(group);
                }
            }

            Time = 0;
            RaisePropertyChanged(nameof(EndDate)); // ToDo: raise only if changed
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
