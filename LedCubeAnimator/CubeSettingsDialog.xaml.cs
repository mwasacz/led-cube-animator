using LedCubeAnimator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LedCubeAnimator
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
