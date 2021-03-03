using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundLevelCalculator.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public enum Units
        {
            Milimeters,
            Meters,
            CubicMetersPerHour,
            CubicDecimetersPerSecond,
        }

        public NumericUpDown()
        {
            InitializeComponent();
            this.MinValue = this.MaxValue = this.Tick = 0;
        }

        public bool HasError
        {
            get
            {
                return Validation.GetHasError(UpDownTextBoxControl);
            }
        }

        public Units Unit
        {
            get
            {
                return (Units)GetValue(UnitProperty);
            }
            set
            {
                var arg = new RoutedPropertyChangedEventArgs<Units>(Unit, value);
                SetValue(UnitProperty, value);
                OnUnitChanged(this, arg);
            }
        }

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(Units), typeof(NumericUpDown));

        public double MinValue
        {
            get
            {
                return (double)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceMinValueProperty));

        private static object OnCoerceMinValueProperty(DependencyObject sender, object data)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            double current = (double)data;

            if (current < 0)
            {
                current = 0;
            }

            return current;
        }

        public double MaxValue
        {
            get
            {
                return (double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceMaxValueProperty));

        private static object OnCoerceMaxValueProperty(DependencyObject sender, object data)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            double current = (double)data;

            if (current < numericUpDown.MinValue)
            {
                current = numericUpDown.MinValue;
            }

            if (current < Convert.ToDouble(numericUpDown.Value))
            {
                numericUpDown.Value = current;
            }

            return current;
        }

        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static object OnCoerceValueProperty(DependencyObject sender, object data)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            var current = Convert.ToDouble(data.ToString().Replace(",", "."), CultureInfo.InvariantCulture);
            double temp;

            if (current <= numericUpDown.MinValue)
            {
                current = numericUpDown.MinValue;
            }

            try
            {
                if (numericUpDown.Unit == Units.Meters)
                {
                    current *= 100D;
                }

                temp = Math.Round(current / numericUpDown.Tick, MidpointRounding.AwayFromZero) * numericUpDown.Tick;
                current = Convert.ToInt32(temp);

                if (numericUpDown.Unit == Units.Meters)
                {
                    current /= 100D;
                }
            }
            catch { }

            if (current >= numericUpDown.MaxValue)
            {
                current = numericUpDown.MaxValue;
            }

            return current;
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceValueProperty));

        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnChangeMaxLength));

        private static object OnChangeMaxLength(DependencyObject sender, object data)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            numericUpDown.UpDownTextBoxControl.MaxLength = (int)data;

            return numericUpDown.UpDownTextBoxControl.MaxLength;
        }

        public double Tick
        {
            get
            {
                return (double)GetValue(TickProperty);
            }
            set
            {
                SetValue(TickProperty, value);
            }
        }

        private static object OnCoerceTickProperty(DependencyObject sender, object data)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            double current = (double)data;

            if (current < 0)
            {
                current = 0;
            }

            return current;
        }

        public static readonly DependencyProperty TickProperty =
            DependencyProperty.Register("Tick", typeof(double), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceTickProperty));

        private void UpDownTextBoxControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
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
                case Key.D8:
                case Key.D9:
                case Key.Left:
                case Key.Right:
                case Key.Tab:
                case Key.Back:
                    e.Handled = false;
                    break;
                case Key.OemComma:
                case Key.OemPeriod:
                    if (this.Unit != Units.Meters)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        if ((UpDownTextBoxControl.Text.Contains(",") || UpDownTextBoxControl.Text.Contains(".")) &&
                            !UpDownTextBoxControl.SelectedText.Contains(e.Key == Key.OemComma ? "." : ","))
                        {
                            if (!UpDownTextBoxControl.SelectedText.Contains(e.Key == Key.OemComma ? "," : "."))
                            {
                                e.Handled = true;
                            }
                        }
                    }

                    break;
                case Key.Up:
                    Button_MouseDown(UpButton, null);
                    break;
                case Key.Down:
                    Button_MouseDown(DownButton, null);
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender.Equals(UpButton))
            {
                if (this.Value + this.Tick <= this.MaxValue)
                {
                    this.Value += this.Tick;
                }
            }
            else if (sender.Equals(DownButton))
            {
                if (this.Value - this.Tick >= this.MinValue)
                {
                    this.Value -= this.Tick;
                }
            }
        }

        private void OnUnitChanged(object sender, RoutedPropertyChangedEventArgs<Units> e)
        {
            var arg = new RoutedPropertyChangedEventArgs<Units>(e.OldValue, e.NewValue, UnitChangedEvent);
            RaiseEvent(arg);
        }

        public event RoutedPropertyChangedEventHandler<Units> UnitChanged
        {
            add { AddHandler(UnitChangedEvent, value); }
            remove { RemoveHandler(UnitChangedEvent, value); }
        }

        public static readonly RoutedEvent UnitChangedEvent =
            EventManager.RegisterRoutedEvent("UnitChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Units>), typeof(NumericUpDown));

        private void UpDownTextBoxControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UpDownTextBoxControl.Text))
            {
                UpDownTextBoxControl.Text = UpDownTextBoxControl.Text.Replace(",", ".");

                if (UpDownTextBoxControl.Text.IndexOf(".") == 0)
                {
                    UpDownTextBoxControl.Text = UpDownTextBoxControl.Text.Replace(".", "0.");
                }

                this.Value = Convert.ToDouble(OnCoerceValueProperty(this, UpDownTextBoxControl.Text));
            }
        }
    }
}
