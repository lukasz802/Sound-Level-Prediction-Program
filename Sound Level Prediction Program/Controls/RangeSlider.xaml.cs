﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sound_Level_Prediction_Program.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy RangeSlider.xaml
    /// </summary>
    public partial class RangeSlider : UserControl
    {
        public RangeSlider()
        {
            InitializeComponent();
            this.Loaded += RangeSlider_Loaded;
            _margin = new Thickness(0);
        }

        private Thickness _margin;
        private RoutedEventArgs _args;

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent != null) { throw new InvalidOperationException("You cannot change a content of this control"); }
        }
        
        private void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            e.Handled = true;
            _args = (sender == LowerSlider) ? new RoutedEventArgs(LowerValueChangedEvent) : new RoutedEventArgs(UpperValueChangedEvent);
            RaiseEvent(_args);
        }

        private void RangeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            LowerSlider.ValueChanged += LowerSlider_ValueChanged;
            UpperSlider.ValueChanged += UpperSlider_ValueChanged;
            UpperSlider.ValueChanged += OnValueChanged;
            LowerSlider.ValueChanged += OnValueChanged;
            LowerSlider_ValueChanged(sender, null);
        }

        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpperSlider.Value = Math.Max(UpperSlider.Value, LowerSlider.Value);
            ValueBorder.Width = Convert.ToDouble(UpperSlider.Value * AreaBorder.ActualWidth / Maximum - LowerSlider.Value * AreaBorder.ActualWidth / Maximum);
            _margin.Left = LowerSlider.Value * AreaBorder.ActualWidth / Maximum;
            ValueBorder.Margin = _margin;
        }

        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSlider.Value = Math.Min(UpperSlider.Value, LowerSlider.Value);
            ValueBorder.Width = Convert.ToDouble(UpperSlider.Value * AreaBorder.ActualWidth / Maximum - LowerSlider.Value * AreaBorder.ActualWidth / Maximum);
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceMinimumProperty));

        private static object OnCoerceMinimumProperty(DependencyObject sender, object data)
        {
            RangeSlider rangeSlider = (RangeSlider)sender;
            double current = (double)data;

            if (current < 0) { current = 0; }
            return current;
        }

        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        private static object OnCoerceLowerValueProperty(DependencyObject sender, object data)
        {
            RangeSlider rangeSlider = (RangeSlider)sender;
            double current = (double)data;

            if (current < rangeSlider.Minimum) { current = rangeSlider.Minimum; }
            if (current > rangeSlider.Maximum) { current = rangeSlider.Maximum; }
            return current;
        }

        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceLowerValueProperty));

        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }

        private static object OnCoerceUpperValueProperty(DependencyObject sender, object data)
        {
            RangeSlider rangeSlider = (RangeSlider)sender;
            double current = (double)data;

            if (current < rangeSlider.LowerValue) { current = rangeSlider.LowerValue; }
            if (current > rangeSlider.Maximum) { current = rangeSlider.Maximum; }
            return current;
        }

        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceUpperValueProperty));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static object OnCoerceMaximumProperty(DependencyObject sender, object data)
        {
            RangeSlider rangeSlider = (RangeSlider)sender;
            double current = (double)data;

            if (current < rangeSlider.Minimum) { current = rangeSlider.Minimum; }
            return current;
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider),
            new FrameworkPropertyMetadata(propertyChangedCallback: null, coerceValueCallback: OnCoerceMaximumProperty));

        public Brush TrackBackground
        {
            get { return (Brush)GetValue(TrackBackgroundProperty); }
            set { SetValue(TrackBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty TrackBackgroundProperty =
            DependencyProperty.Register("TrackBackground", typeof(Brush), typeof(RangeSlider));

        public Brush ValueTrackBackground
        {
            get { return (Brush)GetValue(ValueTrackBackgroundProperty); }
            set { SetValue(ValueTrackBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ValueTrackBackgroundProperty =
            DependencyProperty.Register("ValueTrackBackground", typeof(Brush), typeof(RangeSlider));

        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), typeof(RangeSlider));

        public event RoutedEventHandler LowerValueChanged
        {
            add { AddHandler(LowerValueChangedEvent, value); }
            remove { RemoveHandler(LowerValueChangedEvent, value); }
        }

        public static readonly RoutedEvent LowerValueChangedEvent =
            EventManager.RegisterRoutedEvent("LowerValueChanged",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RangeSlider));

        public event RoutedEventHandler UpperValueChanged
        {
            add { AddHandler(UpperValueChangedEvent, value); }
            remove { RemoveHandler(UpperValueChangedEvent, value); }
        }

        public static readonly RoutedEvent UpperValueChangedEvent =
            EventManager.RegisterRoutedEvent("UpperValueChanged",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RangeSlider));
    }
}
