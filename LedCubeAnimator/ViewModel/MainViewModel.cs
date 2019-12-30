using GalaSoft.MvvmLight;
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
            AddColorToRecents(Colors.Black);
        }

        private AnimationViewModel _animation;

        private void Animation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AnimationViewModel.ColorMode))
            {
                RaisePropertyChanged(nameof(ColorMode));
            }
            else if (e.PropertyName == nameof(AnimationViewModel.MonoColor))
            {
                RenderFrame();
            }
        }

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
                    RaisePropertyChanged(nameof(StartDate));
                    RaisePropertyChanged(nameof(EndDate));
                    ClampTime();
                }
            }
        }

        public TileViewModel SelectedTileOrGroup => SelectedTile ?? SelectedGroup;

        private void SelectedGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GroupViewModel.Start) || e.PropertyName == nameof(GroupViewModel.End))
            {
                RaisePropertyChanged(nameof(StartDate));
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

        public ColorMode ColorMode => _animation.ColorMode;

        private byte _selectedBrightness;
        public byte SelectedBrightness
        {
            get => _selectedBrightness;
            set
            {
                _selectedBrightness = value;

                var color = _animation.MonoColor;
                SelectedColor = new Color
                {
                    A = 255,
                    R = (byte)(color.R * _selectedBrightness / 255),
                    G = (byte)(color.G * _selectedBrightness / 255),
                    B = (byte)(color.B * _selectedBrightness / 255)
                };

                RaisePropertyChanged(nameof(SelectedBrightness));
            }
        }

        private Color _selectedColor;
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                _selectedRecentColor = RecentColors.SingleOrDefault(c => c.Color == _selectedColor);
                RaisePropertyChanged(nameof(SelectedColor));
                RaisePropertyChanged(nameof(SelectedRecentColor));
            }
        }

        private ColorItem _selectedRecentColor;
        public ColorItem SelectedRecentColor
        {
            get => _selectedRecentColor;
            set
            {
                _selectedRecentColor = value;
                _selectedColor = _selectedRecentColor?.Color ?? default;
                RaisePropertyChanged(nameof(SelectedColor));
                RaisePropertyChanged(nameof(SelectedRecentColor));
            }
        }

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

        public int StartDate => Groups.Last().Start;

        public int EndDate => Groups.Last().End;

        private RelayCommand _addFrameCommand;
        public ICommand AddFrameCommand => _addFrameCommand ?? (_addFrameCommand = new RelayCommand(() =>
        {
            var frame = new FrameViewModel(new Frame { Name = "Frame", Voxels = new Color[4, 4, 4] });
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
                MonoColor = _animation.MonoColor
            };
            var dialog = new CubeSettingsDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                _animation.Size = viewModel.Size;
                _animation.ColorMode = viewModel.ColorMode;
                _animation.MonoColor = viewModel.MonoColor;
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
                    SelectedColor = frame.Voxels[index];
                    ColorPickerTool = false;
                }
                else
                {
                    switch (_animation.ColorMode)
                    {
                        case ColorMode.Mono:
                            frame.Voxels[index] = (Colors.White - frame.Voxels[index]).Opaque();
                            break;
                        case ColorMode.MonoBrightness:
                            frame.Voxels[index] = new Color
                            {
                                A = 255,
                                R = SelectedBrightness,
                                G = SelectedBrightness,
                                B = SelectedBrightness
                            };
                            break;
                        case ColorMode.RGB:
                            frame.Voxels[index] = SelectedColor;
                            break;
                    }
                    AddColorToRecents(SelectedColor);
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
            Frame = Renderer.Render(_animation.Animation, Time);
        }

        private void AddColorToRecents(Color color)
        {
            var sameColor = RecentColors.SingleOrDefault(c => c.Color == color);
            if (sameColor != null)
            {
                RecentColors.Move(RecentColors.IndexOf(sameColor), 0);
            }
            else
            {
                RecentColors.Insert(0, new ColorItem(color, color.ToString()));
                if (RecentColors.Count > 10)
                {
                    RecentColors.RemoveAt(10);
                }
            }
            SelectedRecentColor = RecentColors[0];
        }

        private void CreateDefaultAnimation()
        {
            var g = new Group { Name = "MainGroup" };
            if (_animation != null)
            {
                _animation.PropertyChanged -= Animation_PropertyChanged;
            }
            _animation = new AnimationViewModel(new Animation { MainGroup = g, Size = 4, MonoColor = Colors.White });
            RaisePropertyChanged(nameof(ColorMode));
            _animation.PropertyChanged += Animation_PropertyChanged;
            var group = new GroupViewModel(g);
            Groups.Clear();
            Groups.Add(group);
            SelectedGroup = group;
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

            Animation a;
            try
            {
                using (var sr = new StreamReader(dialog.FileName))
                {
                    a = JsonConvert.DeserializeObject<Animation>(sr.ReadToEnd(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                }
            }
            catch
            {
                MessageBox.Show("Error occured when opening file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _filePath = dialog.FileName;

            if (_animation != null)
            {
                _animation.PropertyChanged -= Animation_PropertyChanged;
            }
            _animation = new AnimationViewModel(a);
            _animation.PropertyChanged += Animation_PropertyChanged;
            RaisePropertyChanged(nameof(ColorMode));
            var group = new GroupViewModel(_animation.MainGroup);
            Groups.Clear();
            Groups.Add(group);
            SelectedGroup = group;
        }));

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() =>
        {
            if (_filePath != null)
            {
                Save();
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

            Save();
        }));

        private void Save()
        {
            using (var sw = new StreamWriter(_filePath))
            {
                sw.Write(JsonConvert.SerializeObject(_animation.Animation, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
            }
        }
    }
}
