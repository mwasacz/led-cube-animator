using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.ViewModel.DataViewModels;
using LedCubeAnimator.ViewModel.UserControlViewModels;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LedCubeAnimator.ViewModel.WindowViewModels
{
    public class MainViewModel : ViewModelBase, ISharedViewModel
    {
        public MainViewModel(IModelManager model, IMessenger messenger, IViewModelFactory viewModelFactory, IDialogService dialogService) : base(messenger)
        {
            Model = model;
            _viewModelFactory = viewModelFactory;
            _dialogService = dialogService;

            Model.AnimationChanged += Model_AnimationChanged;
            Model.PropertiesChanged += Model_PropertiesChanged;

            MessengerInstance.Register<PropertyChangedMessage<bool>>(this, HandlePropertyChangedMessage);

            UpdateAnimationViewModel();

            Cube3DViewModel = new Cube3DViewModel(Model, this);
            TimelineViewModel = new TimelineViewModel(Model, this);
            ColorPickerViewModel = new ColorPickerViewModel(Model, this);
        }

        public IModelManager Model { get; }

        private const string _dataFormat = nameof(LedCubeAnimator);

        private readonly IViewModelFactory _viewModelFactory;
        private readonly IDialogService _dialogService;

        private bool _unsavedChanges;

        public Cube3DViewModel Cube3DViewModel { get; }
        public TimelineViewModel TimelineViewModel { get; }
        public ColorPickerViewModel ColorPickerViewModel { get; }

        public AnimationViewModel AnimationViewModel { get; private set; }

        private GroupViewModel _currentGroup;
        public GroupViewModel CurrentGroup
        {
            get => _currentGroup;
            private set => Set(ref _currentGroup, value);
        }

        public ObservableCollection<TileViewModel> SelectedTiles { get; } = new ObservableCollection<TileViewModel>();

        private void HandlePropertyChangedMessage(PropertyChangedMessage<bool> message)
        {
            if (message.Sender is TileViewModel tile)
            {
                if (message.PropertyName == nameof(TileViewModel.Selected))
                {
                    if (tile.Selected)
                    {
                        foreach (var t in SelectedTiles.Where(t => t.Parent != tile.Parent).ToArray())
                        {
                            t.Selected = false;
                        }
                        SelectedTiles.Add(tile);
                    }
                    else
                    {
                        SelectedTiles.Remove(tile);
                    }
                    Application.Current.Dispatcher.BeginInvoke((Action)(() => UpdateCurrentGroup())); // ToDo
                }
                else if (tile is GroupViewModel group && message.PropertyName == nameof(GroupViewModel.Expanded))
                {
                    UpdateCurrentGroup();
                }
            }
        }

        private void UpdateCurrentGroup()
        {
            CurrentGroup = SelectedTiles.Count == 1 && SelectedTiles[0] is GroupViewModel group && group.Expanded
                ? group
                : SelectedTiles.FirstOrDefault()?.Parent ?? AnimationViewModel;
            Time = SelectedTiles.FirstOrDefault() == CurrentGroup
                ? 0
                : SelectedTiles.FirstOrDefault()?.Start ?? 0;
        }

        private object _selectedToolWindow;
        public object SelectedToolWindow
        {
            get => _selectedToolWindow;
            set
            {
                if (Set(ref _selectedToolWindow, value))
                {
                    Model.MergeAllowed = false;
                    Model.MergeAllowed = true;
                }
            }
        }

        private string _selectedProperty;
        public string SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                if (Set(ref _selectedProperty, value))
                {
                    if (SelectedTiles.Count == 1 && SelectedTiles[0] is GroupViewModel group)
                    {
                        if (_selectedProperty == nameof(GroupViewModel.Children))
                        {
                            group.Expanded = true;
                        }
                        else if (group != AnimationViewModel && (_selectedProperty == nameof(GroupViewModel.Start)
                            || _selectedProperty == nameof(GroupViewModel.End)
                            || _selectedProperty == nameof(GroupViewModel.Channel)
                            || _selectedProperty == nameof(GroupViewModel.Hierarchy)))
                        {
                            group.Expanded = false;
                        }
                    }
                    Model.MergeAllowed = false;
                    Model.MergeAllowed = true;
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
            set => Set(ref _time, Math.Max(0, Math.Min(value, CurrentGroup.Columns - 1)));
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

        private RelayCommand _cutCommand;
        public ICommand CutCommand => _cutCommand ?? (_cutCommand = new RelayCommand(() =>
        {
            if (CanCopyTiles())
            {
                string data = Model.Copy(GetTilesToCopy().Select(t => t.Tile).ToArray());
                Clipboard.SetData(_dataFormat, data);
                DeleteTiles(GetTilesToCopy().ToArray());
            }
        }, CanCopyTiles));

        private RelayCommand _copyCommand;
        public ICommand CopyCommand => _copyCommand ?? (_copyCommand = new RelayCommand(() =>
        {
            if (CanCopyTiles())
            {
                string data = Model.Copy(GetTilesToCopy().Select(t => t.Tile).ToArray());
                Clipboard.SetData(_dataFormat, data);
            }
        }, CanCopyTiles));

        private RelayCommand _pasteCommand;
        public ICommand PasteCommand => _pasteCommand ?? (_pasteCommand = new RelayCommand(() =>
        {
            if (Clipboard.GetData(_dataFormat) is string data && Model.Paste(CurrentGroup.Group, data))
            {
                Model.MergeAllowed = false;
                Model.MergeAllowed = true;
            }
        }, () => Clipboard.ContainsData(_dataFormat)));

        private RelayCommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new RelayCommand(() =>
        {
            if (CanCopyTiles())
            {
                DeleteTiles(GetTilesToCopy().ToArray());
            }
        }, CanCopyTiles));

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

        private void AddTile(Tile tile)
        {
            Model.AddTile(CurrentGroup.Group, tile);
            Model.MergeAllowed = false;
            Model.MergeAllowed = true;
        }

        private void DeleteTiles(ICollection<TileViewModel> tiles)
        {
            Model.Group(() =>
            {
                foreach (var tile in tiles)
                {
                    Model.RemoveTile(tile.Parent.Group, tile.Tile);
                }
            });
            Model.MergeAllowed = false;
            Model.MergeAllowed = true;
        }

        private IEnumerable<TileViewModel> GetTilesToCopy() => SelectedTiles.Where(t => t != AnimationViewModel);

        private bool CanCopyTiles() => GetTilesToCopy().Any();

        private void UpdateAnimationViewModel()
        {
            AnimationViewModel?.Cleanup();

            AnimationViewModel = (AnimationViewModel)_viewModelFactory.Create(Model.Animation, (GroupViewModel)null);
            AnimationViewModel.Selected = true;
            UpdateCurrentGroup();

            SelectedColor = Colors.Black;
        }

        private void Model_AnimationChanged(object sender, EventArgs e)
        {
            UpdateAnimationViewModel();
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            if (e.Changes.Any(c => c.Key is Animation && (c.Value == nameof(Animation.ColorMode) || c.Value == nameof(Animation.MonoColor))))
            {
                SelectedColor = Colors.Black;
            }

            _unsavedChanges = true;
        }
    }
}
