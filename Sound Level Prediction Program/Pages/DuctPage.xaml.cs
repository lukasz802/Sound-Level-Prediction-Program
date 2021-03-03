using SoundLevelCalculator.Controls;
using SoundLevelCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using static SoundLevelCalculator.Controls.NumericUpDown;
using static SoundLevelCalculator.SharedFunctions;

namespace SoundLevelCalculator.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy DuctPage.xaml
    /// </summary>
    public partial class DuctPage : Page
    {
        #region Fields and Constants

        private DuctElementViewModel viewModel = new DuctElementViewModel();
        private List<KeyValuePair<Button, List<Image>>> argRoundList;
        private List<KeyValuePair<Button, List<Image>>> argRectangularList;
        private System.ComponentModel.ListSortDirection sortRoundValue;
        private System.ComponentModel.ListSortDirection sortRectangularValue;
        private Button previuosClickedButton;

        #endregion

        #region Constructor

        public DuctPage()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            this.Loaded += DuctPage_Loaded;
            this.Loaded += LoadAreaChartData;
        }

        #endregion

        #region Methods

        private void LoadAreaChartData(object sender, RoutedEventArgs e)
        {
            ((LineSeries)AcousticAttenuationChart.Series[0]).ItemsSource = viewModel.NoiseLevelList;
            ((LineSeries)AcousticAttenuationChart.Series[1]).ItemsSource = viewModel.AttenuationLevelList;
            UpdateChartAxesRange();
        }

        private Tuple<double, double> GetDesiredAxisParameters(ObservableCollection<KeyValuePair<int, double>> acousticData)
        {
            var maxValue = acousticData.Select(x => x.Value).ToList().Max();
            var minValue = acousticData.Select(x => x.Value).ToList().Min();

            return new Tuple<double, double>(minValue, maxValue);
        }

        private void UpdateChartAxesRange()
        {
            var noisleLevelData = GetDesiredAxisParameters(viewModel.NoiseLevelList);
            var attenuationData = GetDesiredAxisParameters(viewModel.AttenuationLevelList);
            var maxValue = Math.Max(attenuationData.Item2, noisleLevelData.Item2);
            var minValue = Math.Min(attenuationData.Item1, noisleLevelData.Item1);
            var range = Math.Round(1.25 * Math.Abs(maxValue - minValue) / 10D, 0, MidpointRounding.AwayFromZero);
            var divider = 0;

            while (range > 10)
            {
                divider++;
                range = Math.Round(1.25 * Math.Abs(maxValue - minValue) / (10D + divider), 0, MidpointRounding.AwayFromZero);
            }

            var linearAxis = (LinearAxis)AcousticAttenuationChart.Axes.Where(x => x.Orientation == AxisOrientation.Y).FirstOrDefault();

            linearAxis.Interval = range;
            var newMinimum = range * (Math.Round(minValue / range, MidpointRounding.AwayFromZero) - 1);
            var newMaximum = range * (Math.Round(maxValue / range, MidpointRounding.AwayFromZero) + 1);

            if (newMinimum < linearAxis.Maximum)
            {
                linearAxis.Minimum = newMinimum;
                linearAxis.Maximum = newMaximum;
            }
            else
            {
                linearAxis.Maximum = newMaximum;
                linearAxis.Minimum = newMinimum;
            }
        }

        private void DuctPage_Loaded(object sender, RoutedEventArgs e)
        {
            sortRoundValue = sortRectangularValue = System.ComponentModel.ListSortDirection.Ascending;
            previuosClickedButton = WidthButton;

            argRoundList = new List<KeyValuePair<Button, List<Image>>>()
            {
                new KeyValuePair<Button, List<Image>>( DiameterButton, new List<Image>() { VelocityUpImage, VelocityDownImage }),
                new KeyValuePair<Button, List<Image>>( VelocityButton, new List<Image>() { DiameterUpImage, DiameterDownImage }),
            };

            argRectangularList = new List<KeyValuePair<Button, List<Image>>>()
            {
                new KeyValuePair<Button, List<Image>>( WidthButton, new List<Image>() { WidthUpImage, WidthDownImage }),
                new KeyValuePair<Button, List<Image>>( HeightButton, new List<Image>() { HeightUpImage, HeightDownImage }),
                new KeyValuePair<Button, List<Image>>( RectangularVelocityButton, new List<Image>() { RectangularVelocityUpImage, RectangularVelocityDownImage }),
            };

            RoundDimensionsListBox.IsVisibleChanged += RoundDimensionsListBox_IsVisibleChanged;
            ElementProfileComboBox.SelectionChanged += ElementProfileComboBox_SelectionChanged;
            RectangularDimensionsListBox.SelectionChanged += (s, o) => UpdateChartAxesRange();
            RoundDimensionsListBox.SelectionChanged += (s, o) => UpdateChartAxesRange();
            viewModel.PropertyChanged += (s, o) => UpdateChartAxesRange();
        }

        private void ElementProfileComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemName = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
            if (itemName == "Okrągły")
            {
                HeightTextBox.Visibility = Visibility.Hidden;
                HeightGrid.Visibility = Visibility.Hidden;
                DiameterTextBox.Visibility = Visibility.Visible;
                DiameterGrid.Visibility = Visibility.Visible;
                WidthTextBox.Visibility = Visibility.Collapsed;
                WidthGrid.Visibility = Visibility.Collapsed;
                RoundDuctScheme.Visibility = Visibility.Visible;
                RectangularDuctScheme.Visibility = Visibility.Hidden;
                ElementIsolationNumericUpDown.MaxValue = 75;
                viewModel.DuctType = Compute_Engine.Enums.DuctType.Round;
            }
            else
            {
                HeightGrid.Visibility = Visibility.Visible;
                HeightTextBox.Visibility = Visibility.Visible;
                DiameterTextBox.Visibility = Visibility.Hidden;
                DiameterGrid.Visibility = Visibility.Hidden;
                WidthTextBox.Visibility = Visibility.Visible;
                WidthGrid.Visibility = Visibility.Visible;
                RoundDuctScheme.Visibility = Visibility.Hidden;
                RectangularDuctScheme.Visibility = Visibility.Visible;
                ElementIsolationNumericUpDown.MaxValue = 50;
                viewModel.DuctType = Compute_Engine.Enums.DuctType.Rectangular;
            }
        }

        private void DimensionButton_Click(object sender, RoutedEventArgs e)
        {
            bool temp;

            if (((Button)sender).Equals(DiameterButton))
            {
                temp = false;
            }
            else
            {
                temp = true;
            }

            foreach (Image img in argRoundList[Convert.ToInt32(temp)].Value)
            {
                img.Visibility = Visibility.Hidden;
            }

            RoundDimensionsListBox.Items.SortDescriptions.Clear();
            RoundDimensionsListBox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Velocity", sortRoundValue));

            if (sortRoundValue == System.ComponentModel.ListSortDirection.Descending)
            {
                sortRoundValue = System.ComponentModel.ListSortDirection.Ascending;
                argRoundList[Convert.ToInt32(!temp)].Value[0].Visibility = Visibility.Visible;
                argRoundList[Convert.ToInt32(!temp)].Value[1].Visibility = Visibility.Hidden;
            }
            else
            {
                sortRoundValue = System.ComponentModel.ListSortDirection.Descending;
                argRoundList[Convert.ToInt32(!temp)].Value[0].Visibility = Visibility.Hidden;
                argRoundList[Convert.ToInt32(!temp)].Value[1].Visibility = Visibility.Visible;
            }
        }

        private void RectagularDimensionButton_Click(object sender, RoutedEventArgs e)
        {
            string tempProperty = string.Empty;
            if (((Button)sender).Equals(WidthButton))
            {
                tempProperty = "Width";
            }
            if (((Button)sender).Equals(HeightButton))
            {
                tempProperty = "Height";
            }
            else if (((Button)sender).Equals(RectangularVelocityButton))
            {
                tempProperty = "Velocity";
            }

            RectangularDimensionsListBox.Items.SortDescriptions.Clear();

            foreach (KeyValuePair<Button, List<Image>> kvp in argRectangularList)
            {
                if (!((Button)sender).Equals(kvp.Key))
                {
                    foreach (Image img in kvp.Value)
                    {
                        img.Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    if (!previuosClickedButton.Equals((Button)sender))
                    {
                        kvp.Value[0].Visibility = Visibility.Visible;
                        kvp.Value[1].Visibility = Visibility.Hidden;
                        sortRectangularValue = System.ComponentModel.ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (sortRectangularValue == System.ComponentModel.ListSortDirection.Ascending)
                        {
                            kvp.Value[0].Visibility = Visibility.Hidden;
                            kvp.Value[1].Visibility = Visibility.Visible;
                            sortRectangularValue = System.ComponentModel.ListSortDirection.Descending;
                        }
                        else
                        {
                            kvp.Value[0].Visibility = Visibility.Visible;
                            kvp.Value[1].Visibility = Visibility.Hidden;
                            sortRectangularValue = System.ComponentModel.ListSortDirection.Ascending;
                        }
                    }

                    RectangularDimensionsListBox.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(tempProperty, sortRectangularValue));
                }
            }

            previuosClickedButton = (Button)sender;
        }

        private void RoundDimensionsListBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            RoundDimensionsListBox.Items.SortDescriptions.Clear();
            argRoundList[0].Value[0].Visibility = Visibility.Hidden;
            argRoundList[0].Value[1].Visibility = Visibility.Hidden;
            argRoundList[1].Value[0].Visibility = Visibility.Visible;
            argRoundList[1].Value[1].Visibility = Visibility.Hidden;
        }

        private void LengthNumericUpDown_LostFocus(object sender, RoutedEventArgs e)
        {
            LengthUnitComboBox.IsEnabled = !((NumericUpDown)sender).HasError;
        }

        private void LengthUnitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LengthNumericUpDown.Unit = ((CommandComboBox)sender).SelectedIndex == 0 ? Units.Milimeters : Units.Meters;
        }

        private void AirVolumeUnitComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AirVolumeNumericUpDown.Unit = ((CommandComboBox)sender).SelectedIndex == 0 ? Units.CubicMetersPerHour : Units.CubicDecimetersPerSecond;
        }

        private void ElementIsolationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.IsIsolated = ((CommandComboBox)sender).SelectedIndex != 0;
        }

        private void AirVolumeNumericUpDown_UnitChanged(object sender, RoutedPropertyChangedEventArgs<Units> e)
        {
            var control = (NumericUpDown)sender;

            if ((e.NewValue == Units.CubicDecimetersPerSecond || e.NewValue == Units.CubicMetersPerHour)
                && (e.OldValue == Units.CubicMetersPerHour || e.OldValue == Units.CubicDecimetersPerSecond))
            {
                control.Tick = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.Tick), e.OldValue, e.NewValue));

                if (e.NewValue == Units.CubicMetersPerHour && e.OldValue == Units.CubicDecimetersPerSecond)
                {
                    control.Tick += 1;
                    control.MaxValue = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.MaxValue), e.OldValue, e.NewValue)) + 1;
                    control.Value = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.Value), e.OldValue, e.NewValue));
                    control.MinValue = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.MinValue), e.OldValue, e.NewValue)) - 1;
                }
                else
                {
                    control.MinValue = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.MinValue), e.OldValue, e.NewValue));
                    control.Value = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.Value), e.OldValue, e.NewValue));
                    control.MaxValue = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.MaxValue), e.OldValue, e.NewValue));
                }
            }

            control.Value = control.Value;
        }

        private void LengthNumericUpDown_UnitChanged(object sender, RoutedPropertyChangedEventArgs<Units> e)
        {
            var control = (NumericUpDown)sender;
            control.Tick = Convert.ToDouble(ConvertUnits(control.Tick, e.OldValue, e.NewValue));

            if (e.NewValue == Units.Milimeters && e.OldValue == Units.Meters)
            {
                control.MaxValue = Convert.ToDouble(ConvertUnits(control.MaxValue, e.OldValue, e.NewValue));
                control.Value = Convert.ToDouble(ConvertUnits(control.Value, e.OldValue, e.NewValue));
                control.MinValue = Convert.ToDouble(ConvertUnits(control.MinValue, e.OldValue, e.NewValue));
            }
            else
            {
                control.MinValue = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.MinValue), e.OldValue, e.NewValue));
                control.Value = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.Value), e.OldValue, e.NewValue));
                control.MaxValue = Convert.ToDouble(ConvertUnits(Convert.ToInt32(control.MaxValue), e.OldValue, e.NewValue));
            }
        }

        private void PnEnDimensionsTypeButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Hidden;
            CustomDimensionsButton.Visibility = Visibility.Visible;
        }

        private void CustomDimensionsButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Visibility = Visibility.Hidden;
            PnEnDimensionsButton.Visibility = Visibility.Visible;
        }
    }

    #endregion
}
