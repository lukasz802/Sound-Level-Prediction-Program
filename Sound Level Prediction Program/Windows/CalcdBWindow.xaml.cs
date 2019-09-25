using Sound_Level_Prediction_Program.ViewModels;
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

namespace Sound_Level_Prediction_Program.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy CalcdBWindow.xaml
    /// </summary>
    public partial class CalcdBWindow : Window
    {
        private CalcdBViewModel viewModel = new CalcdBViewModel();
        private Uri iconUri = new Uri("pack://application:,,,/Icons/calculator_icon.ico", UriKind.RelativeOrAbsolute);  

        public CalcdBWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            this.Icon = BitmapFrame.Create(iconUri);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) { this.Close(); }
        }

        private void MenuBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
