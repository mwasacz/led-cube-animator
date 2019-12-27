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

namespace LedCubeAnimator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            CreateDefaultGroup();
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

        private Color _selectedColor = Colors.Black;
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                RaisePropertyChanged(nameof(SelectedColor));
            }
        }

        public int StartDate => Groups.Last().Start;

        public int EndDate => Groups.Last().End;

        private RelayCommand _addFrameCommand;
        public ICommand AddFrameCommand => _addFrameCommand ?? (_addFrameCommand = new RelayCommand(() =>
        {
            var frame = new FrameViewModel(new Frame { Name = "Frame" });
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
                frame.Voxels[(int)p.X * y * z + (int)p.Y * z + (int)p.Z] = SelectedColor;

                RenderFrame();
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
            var voxels = new Color[4, 4, 4];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int z = 0; z < 4; z++)
                    {
                        voxels[x, y, z] = Groups[0].Group.GetVoxel(new Point3D(x, y, z), _time, (p, t) => Colors.Black);
                    }
                }
            }

            Frame = voxels;
        }

        private void CreateDefaultGroup()
        {
            var group = new GroupViewModel(new Group { Name = "MainGroup" });
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

            CreateDefaultGroup();
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

            Group g;
            try
            {
                using (var sr = new StreamReader(dialog.FileName))
                {
                    g = JsonConvert.DeserializeObject<Group>(sr.ReadToEnd(), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                }
            }
            catch
            {
                MessageBox.Show("Error occured when opening file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _filePath = dialog.FileName;

            var group = new GroupViewModel(g);
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
                sw.Write(JsonConvert.SerializeObject(Groups[0].Group, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects }));
            }
        }

        public ObservableCollection<Xceed.Wpf.Toolkit.ColorItem> ColorList { get; } = new ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>()
        {
            new Xceed.Wpf.Toolkit.ColorItem(Colors.Red, "Red"),
            new Xceed.Wpf.Toolkit.ColorItem(Colors.Green, "Green"),
            new Xceed.Wpf.Toolkit.ColorItem(Colors.Blue, "Blue")
        };
    }
}
