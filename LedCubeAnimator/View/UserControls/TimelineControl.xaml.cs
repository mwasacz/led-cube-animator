using LedCubeAnimator.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LedCubeAnimator.View.UserControls
{
    /// <summary>
    /// Interaction logic for TimelineControl.xaml
    /// </summary>
    public partial class TimelineControl : UserControl
    {
        public TimelineControl()
        {
            InitializeComponent();
            SetBinding(ItemDraggedCommandProperty, new Binding("ItemDraggedCommand"));
        }



        public double CellWidth
        {
            get => (double)GetValue(CellWidthProperty);
            set => SetValue(CellWidthProperty, value);
        }

        public static readonly DependencyProperty CellWidthProperty = DependencyProperty.Register("CellWidth",
            typeof(double), typeof(TimelineControl), new PropertyMetadata((double)80));



        public double CellHeight
        {
            get => (double)GetValue(CellHeightProperty);
            set => SetValue(CellHeightProperty, value);
        }

        public static readonly DependencyProperty CellHeightProperty = DependencyProperty.Register("CellHeight",
            typeof(double), typeof(TimelineControl), new PropertyMetadata((double)40));



        public ICommand ItemDraggedCommand
        {
            get => (ICommand)GetValue(ItemDraggedCommandProperty);
            set => SetValue(ItemDraggedCommandProperty, value);
        }

        public static readonly DependencyProperty ItemDraggedCommandProperty = DependencyProperty.Register("ItemDraggedCommand",
            typeof(ICommand), typeof(TimelineControl));



        private DragMode _dragMode;
        private int _lastX;
        private int _lastY;
        private int _handleOffset;

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = (ListBoxItem)sender;
            var pos = e.GetPosition(item);
            double left = Canvas.GetLeft(item);
            double top = Canvas.GetTop(item);
            _lastX = (int)((pos.X + left) / CellWidth);
            _lastY = (int)((pos.Y + top) / CellHeight);
            _handleOffset = (int)(pos.X / CellWidth);

            double handleWidth = (double)Resources["ResizeHandleWidth"];
            if (pos.X < handleWidth)
            {
                _dragMode = DragMode.Left;
            }
            else if (pos.X >= item.ActualWidth - handleWidth)
            {
                _dragMode = DragMode.Right;
            }
            else
            {
                _dragMode = DragMode.Move;
            }
        }

        private void ListBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var item = (ListBoxItem)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                item.Cursor = _dragMode == DragMode.Move ? Cursors.SizeAll : Cursors.SizeWE;

                var pos = e.GetPosition(item);
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);
                int x = (int)((pos.X + left) / CellWidth);
                int y = (int)((pos.Y + top) / CellHeight);

                if (x != _lastX || y != _lastY)
                {
                    var args = new ItemDraggedEventArgs(item.DataContext, _dragMode, x, y, _handleOffset);
                    if (ItemDraggedCommand?.CanExecute(args) == true)
                    {
                        ItemDraggedCommand?.Execute(args);
                    }
                }

                _lastX = x;
                _lastY = y;

                item.Focus();
                item.CaptureMouse();
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (ListBoxItem)sender;
            if (item.IsMouseCaptureWithin)
            {
                e.Handled = true;
                item.ReleaseMouseCapture();
            }
        }

        private void ListBoxItem_LostMouseCapture(object sender, MouseEventArgs e)
        {
            var item = (ListBoxItem)sender;
            item.Cursor = Cursors.Arrow;
        }

        private void ListBoxItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void ListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var listBox = (ListBox)sender;
            listBox.SelectedItem = null;
        }
    }
}
