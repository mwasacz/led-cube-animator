using GalaSoft.MvvmLight;
using LedCubeAnimator.Model;
using LedCubeAnimator.Model.Animations.Data;
using LedCubeAnimator.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using ColorItem = Xceed.Wpf.Toolkit.ColorItem;

namespace LedCubeAnimator.ViewModel.UserControlViewModels
{
    public class ColorPickerViewModel : ViewModelBase
    {
        public ColorPickerViewModel(IModelManager model, ISharedViewModel shared)
        {
            Model = model;
            Shared = shared;

            Model.AnimationChanged += Model_AnimationChanged;
            Model.PropertiesChanged += Model_PropertiesChanged;
            Shared.PropertyChanged += Shared_PropertyChanged;
            Shared.ColorClicked += Shared_ColorClicked;

            ResetColors();
        }

        public IModelManager Model { get; }
        public ISharedViewModel Shared { get; }

        public ColorMode ColorMode => Model.Animation.ColorMode; // ToDo: consider storing brightness as alpha

        public Color DisplayColor => SelectedColor.Multiply(Model.Animation.MonoColor);

        private Color _selectedColor;
        public Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (Set(ref _selectedColor, value))
                {
                    RaisePropertyChanged(nameof(DisplayColor));
                    RecentColor = _recentColors.SingleOrDefault(p => p.Item2 == _selectedColor)?.Item1;
                    Shared.SelectedColor = _selectedColor;
                }
            }
        }

        private ColorItem _recentColor;
        public ColorItem RecentColor
        {
            get => _recentColor;
            set
            {
                if (Set(ref _recentColor, value) && _recentColor != null)
                {
                    SelectedColor = _recentColors.Single(p => p.Item1 == _recentColor).Item2;
                }
            }
        }

        private readonly List<Tuple<ColorItem, Color>> _recentColors = new List<Tuple<ColorItem, Color>>();
        public ObservableCollection<ColorItem> RecentColors { get; } = new ObservableCollection<ColorItem>();

        public bool ColorPickerTool
        {
            get => !Shared.SelectedColor.HasValue;
            set => Shared.SelectedColor = value ? (Color?)null : SelectedColor;
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
                    new ColorItem(color.Multiply(Model.Animation.MonoColor), color.GetBrightness().ToString());

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

        private void ResetColors()
        {
            SelectedColor = Colors.Black;
            _recentColors.Clear();
            RecentColors.Clear();
            AddColorToRecents(Colors.Black);
            RaisePropertyChanged(nameof(ColorMode)); // ToDo: raise only if necessary
        }

        private void Model_AnimationChanged(object sender, EventArgs e)
        {
            ResetColors();
        }

        private void Model_PropertiesChanged(object sender, PropertiesChangedEventArgs e)
        {
            if (e.Changes.Any(c => c.Key is Animation && (c.Value == nameof(Animation.ColorMode) || c.Value == nameof(Animation.MonoColor))))
            {
                ResetColors();
            }
        }

        private void Shared_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISharedViewModel.SelectedColor))
            {
                if (Shared.SelectedColor.HasValue)
                {
                    SelectedColor = Shared.SelectedColor.Value;
                }
                RaisePropertyChanged(nameof(ColorPickerTool)); //ToDo: raise only if necessary
            }
        }

        private void Shared_ColorClicked(object sender, ColorClickedEventArgs e)
        {
            if (ColorPickerTool)
            {
                SelectedColor = e.Color;
            }
            else
            {
                AddColorToRecents(e.Color);
            }
        }
    }
}
