using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.ViewModel.DataViewModels;
using LedCubeAnimator.ViewModel.UserControlViewModels;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel.WindowViewModels
{
    public class MainViewModel : ViewModelBase, ISharedViewModel
    {
        public MainViewModel(IModelManager model, IViewModelFactory viewModelFactory, IDialogService dialogService)
        {
            Model = model;
            _viewModelFactory = viewModelFactory;
            _dialogService = dialogService;

            Model.AnimationChanged += Model_AnimationChanged;
            Model.PropertiesChanged += Model_PropertiesChanged;

            UpdateMainGroupViewModel();

            Cube3DViewModel = new Cube3DViewModel(Model, this);
            TimelineViewModel = new TimelineViewModel(Model, this);
            ColorPickerViewModel = new ColorPickerViewModel(Model, this);
            PropertyViewModel = new PropertyViewModel(Model, this);
        }

        public IModelManager Model { get; }

        private readonly IViewModelFactory _viewModelFactory;
        private readonly IDialogService _dialogService;

        public Cube3DViewModel Cube3DViewModel { get; }
        public TimelineViewModel TimelineViewModel { get; }
        public ColorPickerViewModel ColorPickerViewModel { get; }
        public PropertyViewModel PropertyViewModel { get; }

        private bool _unsavedChanges;
        private GroupViewModel _mainGroup;

        private TileViewModel _selectedTile;
        public TileViewModel SelectedTile
        {
            get => _selectedTile;
            set
            {
                if (_selectedTile != value)
                {
                    bool expanded = value is GroupViewModel group && (group.Parent == null || IsAncestor(group, _selectedTile));
                    _selectedTile = value;
                    if (SelectedTileExpanded != expanded)
                    {
                        SelectedTileExpanded = expanded;
                    }
                    else
                    {
                        UpdateTime();
                    }
                    RaisePropertyChanged();
                }
            }
        }

        private bool _selectedTileExpanded;
        public bool SelectedTileExpanded
        {
            get => _selectedTileExpanded;
            set
            {
                if (Set(ref _selectedTileExpanded, value))
                {
                    UpdateTime();
                }
            }
        }

        private string _selectedProperty;
        public string SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                if (Set(ref _selectedProperty, value) && SelectedTile is GroupViewModel group && group.Parent != null)
                {
                    if (_selectedProperty == nameof(GroupViewModel.Children))
                    {
                        SelectedTileExpanded = true;
                    }
                    else if (_selectedProperty == nameof(GroupViewModel.Start)
                        || _selectedProperty == nameof(GroupViewModel.End)
                        || _selectedProperty == nameof(GroupViewModel.Channel)
                        || _selectedProperty == nameof(GroupViewModel.Hierarchy))
                    {
                        SelectedTileExpanded = false;
                    }
                }
            }
        }

        private Color? _selectedColor;
        public Color? SelectedColor
        {
            get => _selectedColor;
            set => Set(ref _selectedColor, value);
        }

        public void ColorClick(Color color) => ColorClicked?.Invoke(this, new ColorClickedEventArgs(color));

        public event EventHandler<ColorClickedEventArgs> ColorClicked;

        private int _time;
        public int Time
        {
            get => _time;
            set => Set(ref _time, Math.Max(0, Math.Min(value, Length - 1)));
        }

        private int _length;
        public int Length
        {
            get => _length;
            private set => Set(ref _length, value);
        }

        private RelayCommand _addFrameCommand;
        public ICommand AddFrameCommand => _addFrameCommand ?? (_addFrameCommand = new RelayCommand(() =>
        {
            int size = Model.Animation.Size;
            var voxels = new Color[size, size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        voxels[x, y, z] = Colors.Black;
                    }
                }
            }
            AddTile(new Frame { Name = "Frame", Voxels = voxels });
        }));

        private RelayCommand _addGroupCommand;
        public ICommand AddGroupCommand => _addGroupCommand ?? (_addGroupCommand = new RelayCommand(() => AddTile(new Group { Name = "Group" })));

        private RelayCommand _addGradientEffectCommand;
        public ICommand AddGradientEffectCommand => _addGradientEffectCommand ?? (_addGradientEffectCommand = new RelayCommand(() => AddTile(new GradientEffect { Name = "GradientEffect" })));

        private RelayCommand _addMoveEffectCommand;
        public ICommand AddMoveEffectCommand => _addMoveEffectCommand ?? (_addMoveEffectCommand = new RelayCommand(() => AddTile(new MoveEffect { Name = "MoveEffect" })));

        private RelayCommand _addRotateEffectCommand;
        public ICommand AddRotateEffectCommand => _addRotateEffectCommand ?? (_addRotateEffectCommand = new RelayCommand(() => AddTile(new RotateEffect { Name = "RotateEffect" })));

        private RelayCommand _addScaleEffectCommand;
        public ICommand AddScaleEffectCommand => _addScaleEffectCommand ?? (_addScaleEffectCommand = new RelayCommand(() => AddTile(new ScaleEffect { Name = "ScaleEffect" })));

        private RelayCommand _addShearEffectCommand;
        public ICommand AddShearEffectCommand => _addShearEffectCommand ?? (_addShearEffectCommand = new RelayCommand(() => AddTile(new ShearEffect { Name = "ShearEffect" })));

        private RelayCommand _addLinearDelayCommand;
        public ICommand AddLinearDelayCommand => _addLinearDelayCommand ?? (_addLinearDelayCommand = new RelayCommand(() => AddTile(new LinearDelay { Name = "LinearDelay" })));

        private RelayCommand _addRadialDelayCommand;
        public ICommand AddRadialDelayCommand => _addRadialDelayCommand ?? (_addRadialDelayCommand = new RelayCommand(() => AddTile(new RadialDelay { Name = "RadialDelay" })));

        private RelayCommand _addSphericalDelayCommand;
        public ICommand AddSphericalDelayCommand => _addSphericalDelayCommand ?? (_addSphericalDelayCommand = new RelayCommand(() => AddTile(new SphericalDelay { Name = "SphericalDelay" })));

        private RelayCommand _addAngularDelayCommand;
        public ICommand AddAngularDelayCommand => _addAngularDelayCommand ?? (_addAngularDelayCommand = new RelayCommand(() => AddTile(new AngularDelay { Name = "AngularDelay" })));

        private RelayCommand _addCustomTileCommand;
        public ICommand AddCustomTileCommand => _addCustomTileCommand ?? (_addCustomTileCommand = new RelayCommand(() => AddTile(new CustomTile { Name = "CustomTile" })));

        private RelayCommand _cubeSettingsCommand;
        public ICommand CubeSettingsCommand => _cubeSettingsCommand ?? (_cubeSettingsCommand = new RelayCommand(() => _dialogService.ShowDialog(this, new CubeSettingsViewModel(Model))));

        private RelayCommand _newCommand;
        public ICommand NewCommand => _newCommand ?? (_newCommand = new RelayCommand(() =>
        {
            if (ChangesSaved())
            {
                Model.New();

                _unsavedChanges = false;
            }
        }));

        private RelayCommand _openCommand;
        public ICommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(() =>
        {
            if (ChangesSaved())
            {
                var settings = new OpenFileDialogSettings
                {
                    Filter = "Animation files (*.json)|*.json|All files (*.*)|*.*"
                };
                var result = _dialogService.ShowOpenFileDialog(this, settings);
                if (result == true && !Model.Open(settings.FileName))
                {
                    _dialogService.ShowMessageBox(this, "Error occured when opening file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                _unsavedChanges = false;
            }
        }));

        private RelayCommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() => Save()));

        private RelayCommand _saveAsCommand;
        public ICommand SaveAsCommand => _saveAsCommand ?? (_saveAsCommand = new RelayCommand(() => SaveAs()));

        private RelayCommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ?? (_exportCommand = new RelayCommand(() =>
        {
            var settings = new SaveFileDialogSettings
            {
                Filter = "LED Cube binary files (*.3db)|*.3db|All files (*.*)|*.*",
                OverwritePrompt = true
            };
            var result = _dialogService.ShowSaveFileDialog(this, settings);
            if (result == true)
            {
                Model.Export(settings.FileName);
            }
        }));

        private RelayCommand _exportMWCommand;
        public ICommand ExportMWCommand => _exportMWCommand ?? (_exportMWCommand = new RelayCommand(() =>
        {
            var settings = new SaveFileDialogSettings
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                OverwritePrompt = true
            };
            var result = _dialogService.ShowSaveFileDialog(this, settings);
            if (result == true)
            {
                Model.ExportMW(settings.FileName);
            }
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

        private RelayCommand<CancelEventArgs> _closingCommand;
        public ICommand ClosingCommand => _closingCommand ?? (_closingCommand = new RelayCommand<CancelEventArgs>(e =>
        {
            if (!ChangesSaved())
            {
                e.Cancel = true;
            }
        }));

        private bool ChangesSaved()
        {
            if (_unsavedChanges)
            {
                var result = _dialogService.ShowMessageBox(this, "Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        return Save();
                    case MessageBoxResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            return true;
        }

        private bool Save()
        {
            if (Model.Save())
            {
                _unsavedChanges = false;
                return true;
            }
            return SaveAs();
        }

        private bool SaveAs()
        {
            var settings = new SaveFileDialogSettings
            {
                Filter = "Animation files (*.json)|*.json|All files (*.*)|*.*",
                OverwritePrompt = true
            };
            var result = _dialogService.ShowSaveFileDialog(this, settings);
            if (result == true)
            {
                Model.SaveAs(settings.FileName);
                _unsavedChanges = false;
                return true;
            }
            return false;
        }

        private GroupViewModel GetCurrentGroup() => SelectedTileExpanded ? (GroupViewModel)SelectedTile : SelectedTile.Parent;

        private void AddTile(Tile tile) => Model.AddTile(GetCurrentGroup().Group, tile);

        private static bool IsAncestor(GroupViewModel group, TileViewModel tile)
        {
            return tile.Parent != null && (tile.Parent == group || IsAncestor(group, tile.Parent));
        }

        private void UpdateTime()
        {
            var group = GetCurrentGroup();
            int end = (group.End - group.Start + 1) / group.RepeatCount;
            Length = group.Reverse ? end / 2 : end;

            Time = SelectedTileExpanded ? 0 : SelectedTile.Start;
        }

        private void UpdateMainGroupViewModel()
        {
            _mainGroup = (GroupViewModel)_viewModelFactory.Create(Model.Animation.MainGroup, (GroupViewModel)null);
            SelectedTile = _mainGroup;

            SelectedColor = Colors.Black;

            _undoCommand?.RaiseCanExecuteChanged(); // ToDo: raise if necessary or use CommandWpf
            _redoCommand?.RaiseCanExecuteChanged();
        }

        private void Model_AnimationChanged(object sender, EventArgs e)
        {
            UpdateMainGroupViewModel();
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            TileViewModel tile = null;
            string property = null;
            for (int i = 0; i < e.Changes.Count; i++)
            {
                var change = e.Changes[i];
                _mainGroup.ModelPropertyChanged(change.Key, change.Value, out var t, out var p);
                if (i == 0)
                {
                    tile = t;
                    property = p;
                }
            }

            if (tile != null)
            {
                SelectedTile = tile;
                SelectedProperty = property;
            }

            UpdateTime();

            if (e.Changes.Any(c => c.Key is Animation && (c.Value == nameof(Animation.ColorMode) || c.Value == nameof(Animation.MonoColor))))
            {
                SelectedColor = Colors.Black;
            }

            _undoCommand?.RaiseCanExecuteChanged(); // ToDo: raise if necessary or use CommandWpf
            _redoCommand?.RaiseCanExecuteChanged();

            _unsavedChanges = true;
        }
    }
}
