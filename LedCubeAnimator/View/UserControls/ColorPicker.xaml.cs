using LedCubeAnimator.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LedCubeAnimator.View.UserControls
{
    /// <summary>
    /// Interaction logic for ColorPickerControl.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }



        public byte R
        {
            get => (byte)GetValue(RProperty);
            set => SetValue(RProperty, value);
        }

        public static readonly DependencyProperty RProperty = DependencyProperty.Register("R", typeof(byte), typeof(ColorPicker),
            new FrameworkPropertyMetadata((byte)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRChanged));

        private static void OnRChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnRChanged();

        private void OnRChanged() => SelectedColor = Color.FromRgb(R, SelectedColor.G, SelectedColor.B);



        public byte G
        {
            get => (byte)GetValue(GProperty);
            set => SetValue(GProperty, value);
        }

        public static readonly DependencyProperty GProperty = DependencyProperty.Register("G", typeof(byte), typeof(ColorPicker),
            new FrameworkPropertyMetadata((byte)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGChanged));

        private static void OnGChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnGChanged();

        private void OnGChanged() => SelectedColor = Color.FromRgb(SelectedColor.R, G, SelectedColor.B);



        public byte B
        {
            get => (byte)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        public static readonly DependencyProperty BProperty = DependencyProperty.Register("B", typeof(byte), typeof(ColorPicker),
            new FrameworkPropertyMetadata((byte)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBChanged));

        private static void OnBChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnBChanged();

        private void OnBChanged() => SelectedColor = Color.FromRgb(SelectedColor.R, SelectedColor.G, B);



        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
            new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnSelectedColorChanged();

        private void OnSelectedColorChanged()
        {
            R = SelectedColor.R;
            G = SelectedColor.G;
            B = SelectedColor.B;
            if (SelectedHsvColor.ToRgb() != SelectedColor)
            {
                SelectedHsvColor = SelectedColor.ToHsv();
            }
        }



        public double H
        {
            get => (double)GetValue(HProperty);
            set => SetValue(HProperty, value);
        }

        public static readonly DependencyProperty HProperty = DependencyProperty.Register("H", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHChanged));

        private static void OnHChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnHChanged();

        private void OnHChanged() => SelectedHsvColor = new HsvColor(H, SelectedHsvColor.S, SelectedHsvColor.V);



        public double S
        {
            get => (double)GetValue(SProperty);
            set => SetValue(SProperty, value);
        }

        public static readonly DependencyProperty SProperty = DependencyProperty.Register("S", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSChanged));

        private static void OnSChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnSChanged();

        private void OnSChanged() => SelectedHsvColor = new HsvColor(SelectedHsvColor.H, S, SelectedHsvColor.V);



        public double V
        {
            get => (double)GetValue(VProperty);
            set => SetValue(VProperty, value);
        }

        public static readonly DependencyProperty VProperty = DependencyProperty.Register("V", typeof(double), typeof(ColorPicker),
            new FrameworkPropertyMetadata((double)0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnVChanged));

        private static void OnVChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnVChanged();

        private void OnVChanged() => SelectedHsvColor = new HsvColor(SelectedHsvColor.H, SelectedHsvColor.S, V);



        public HsvColor SelectedHsvColor
        {
            get => (HsvColor)GetValue(SelectedHsvColorProperty);
            set => SetValue(SelectedHsvColorProperty, value);
        }

        public static readonly DependencyProperty SelectedHsvColorProperty = DependencyProperty.Register("SelectedHsvColor", typeof(HsvColor), typeof(ColorPicker),
            new FrameworkPropertyMetadata(new HsvColor(0, 0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedHsvColorChanged));

        private static void OnSelectedHsvColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((ColorPicker)d).OnSelectedHsvColorChanged();

        private void OnSelectedHsvColorChanged()
        {
            H = SelectedHsvColor.H;
            S = SelectedHsvColor.S;
            V = SelectedHsvColor.V;
            UpdateSelector();
            if (SelectedColor.ToHsv() != SelectedHsvColor)
            {
                SelectedColor = SelectedHsvColor.ToRgb();
            }
        }



        private void UpdateColorFromPosition(Point p)
        {
            double w = ColorWheel.ActualWidth / 2;
            double h = ColorWheel.ActualHeight / 2;

            double x = (p.X - w) / w;
            double y = (p.Y - h) / h;

            double angle = Math.Atan2(y, x);
            double dist = Math.Min(Math.Sqrt(x * x + y * y), 1);

            SelectedHsvColor = new HsvColor(angle / Math.PI * 180, dist * 255, V);
        }

        private void UpdateSelector()
        {
            double w = ColorWheel.ActualWidth / 2;
            double h = ColorWheel.ActualHeight / 2;

            double angle = H * Math.PI / 180;

            SelectorTransform.X = Math.Cos(angle) * S * w / 255;
            SelectorTransform.Y = Math.Sin(angle) * S * h / 255;

            var color = new HsvColor(H, S, 255).ToRgb();
            SelectorBrush.Color = color;
            GradientStop.Color = color;
        }



        private void ColorWheel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateColorFromPosition(e.GetPosition(ColorWheel));
            ColorWheel.CaptureMouse();
            e.Handled = true;
        }

        private void ColorWheel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorWheel.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void ColorWheel_MouseMove(object sender, MouseEventArgs e)
        {
            if (ColorWheel.IsMouseCaptured)
            {
                UpdateColorFromPosition(e.GetPosition(ColorWheel));
                e.Handled = true;
            }
        }

        private void ColorWheel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSelector();
        }
    }
}
