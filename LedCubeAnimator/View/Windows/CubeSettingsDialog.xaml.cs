using LedCubeAnimator.ViewModel.WindowViewModels;
using System.Windows;

namespace LedCubeAnimator.View.Windows
{
    /// <summary>
    /// Interaction logic for CubeSettingsDialog.xaml
    /// </summary>
    public partial class CubeSettingsDialog : Window
    {
        public CubeSettingsDialog(CubeSettingsViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
