using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundLevelCalculator.Controls
{
    public class ActiveTextBox : TextBox, INotifyPropertyChanged
    {
        private static FrameworkElement lastClicked;

        public ActiveTextBox()
        {
            this.IsActive = false;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (lastClicked == null)
            {
                this.IsActive = true;
            }
            else if (lastClicked is ActiveTextBox)
            {
                ((ActiveTextBox)lastClicked).IsActive = false;
                this.IsActive = true;
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (!string.IsNullOrEmpty(this.Text) && this.Text != ",")
            {
                this.Text = Convert.ToDouble(this.Text).ToString();
            }
            else
            {
                this.Text = "0";
            }

            lastClicked = this;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.Back:
                case Key.Up:
                case Key.Down:
                case Key.Right:
                case Key.Left:
                case Key.Tab:
                    e.Handled = false;
                    break;
                case Key.OemComma:
                    if (this.SelectedText.Contains(",") || !this.Text.Contains(","))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set
            {
                SetValue(IsActiveProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ActiveTextBox));

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
