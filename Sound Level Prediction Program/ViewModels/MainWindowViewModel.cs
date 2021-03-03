using System.Windows.Input;
using System.Windows;


namespace SoundLevelCalculator.ViewModels
{
    public class MainWindowViewModel
    {
        #region Constructor

        public MainWindowViewModel()
        {
            CloseAppCommand = new RelayCommand((o) => CloseApp());
        }

        #endregion

        #region Properties

        public ICommand CloseAppCommand { get; private set; }

        #endregion

        #region Methods

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
