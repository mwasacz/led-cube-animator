using LedCubeAnimator.ViewModel.WindowViewModels;
using System.Windows;

namespace LedCubeAnimator.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private MainViewModel _viewModel;

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_viewModel.SaveCommand.CanExecute(null))
            {
                switch (MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                {
                    case MessageBoxResult.Yes:
                        _viewModel.SaveCommand.Execute(null);
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}
