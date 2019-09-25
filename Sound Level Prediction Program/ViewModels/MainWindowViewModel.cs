using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Sound_Level_Prediction_Program.ViewModels
{
    public class MainWindowViewModel
    {
        public ICommand ShowCalcdBCommand { get; private set; }
        public ICommand CloseAppCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.ShowCalcdBCommand = new RelayCommand((o) => ShowCalcdB());
            this.CloseAppCommand = new RelayCommand((o) => CloseApp());
        }

        private void ShowCalcdB()
        {
            Windows.CalcdBWindow calcdBWindow = new Windows.CalcdBWindow
            {
                Owner = Application.Current.MainWindow
            };
            calcdBWindow.ShowDialog();
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}
