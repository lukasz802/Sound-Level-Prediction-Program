using System;
using System.Windows;
using System.Windows.Controls;

namespace Sound_Level_Prediction_Program.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy AutoHidableDockPanel.xaml
    /// </summary>
    public partial class AutoHidableDockPanel : UserControl
    {
        public AutoHidableDockPanel()
        {
            InitializeComponent();

            try
            {
                main_width = Application.Current.MainWindow.Width;
                this.Loaded += OnLoaded;
            }
            catch { return; }
        }

        static double main_width;

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent != null) { throw new InvalidOperationException("You cannot change a content of this control"); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.SizeChanged += MainWindow_SizeChanged;
        }
     
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < main_width - RelativeVisibilityLimit * main_width)
            {
                MainPanelContent.Visibility = Visibility.Collapsed;
                CollapsedPanelContent.Visibility = Visibility.Visible;
            }
            else
            {
                MainPanelContent.Visibility = Visibility.Visible;
                CollapsedPanelContent.Visibility = Visibility.Collapsed;
            }
        }

        public Object CollapsedContent
        {
            get { return (Object)GetValue(CollapsedContentProperty); }
            set { SetValue(CollapsedContentProperty, value); }
        }

        public static readonly DependencyProperty CollapsedContentProperty =
            DependencyProperty.Register("CollapsedContent", typeof(Object), typeof(AutoHidableDockPanel));

        public Object MainContent
        {
            get { return (Object)GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }

        public static readonly DependencyProperty MainContentProperty =
            DependencyProperty.Register("MainContent", typeof(Object), typeof(AutoHidableDockPanel));

        public double RelativeVisibilityLimit
        {
            get { return (double)GetValue(RelativeVisibilityLimitProperty); }
            set { SetValue(RelativeVisibilityLimitProperty, value); }
        }

        public static readonly DependencyProperty RelativeVisibilityLimitProperty =
            DependencyProperty.Register("RelativeVisibilityLimit", typeof(double), typeof(AutoHidableDockPanel), 
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceRelativeVisibilityLimitProperty, defaultValue: 0.1));

        private static object OnCoerceRelativeVisibilityLimitProperty(DependencyObject sender, object data)
        {
            AutoHidableDockPanel autoHidableDockPanel = (AutoHidableDockPanel)sender;
            double current = (double)data;

            if (current < 0.0) { current = 0.0; }
            if (current > 1.0) { current = 1.0; }
            return current;
        }
    }
}
