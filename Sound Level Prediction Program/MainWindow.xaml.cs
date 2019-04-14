﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sound_Level_Prediction_Program
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int elementslistviewCounter = 4;

        public MainWindow()
        {
            InitializeComponent();
            ElementsListView.SelectionChanged += ElementsListView_SelectionChanged;
        }

        private void ElementsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListView)sender).SelectedIndex == 0)
            { elementslistviewCounter = 4; }
            else if (((ListView)sender).SelectedIndex == ((ListView)sender).Items.Count - 1)
            { elementslistviewCounter = ((ListView)sender).Items.Count - 5; }
            else if (((ListView)sender).SelectedIndex <= ((ListView)sender).Items.Count - 6 && ((ListView)sender).SelectedIndex >= 5)
            { elementslistviewCounter = ((ListView)sender).SelectedIndex; }

            if (((ListView)sender).SelectedIndex < 5 && elementslistviewCounter < 5)
            {
                DownButton.IsEnabled = true;
                UpButton.IsEnabled = false;
            }
            else if (((ListView)sender).SelectedIndex >= ((ListView)sender).Items.Count - 5 && elementslistviewCounter >= ((ListView)sender).Items.Count - 5)
            {
                UpButton.IsEnabled = true;
                DownButton.IsEnabled = false;
            }
            else
            {
                UpButton.IsEnabled = true;
                DownButton.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == DownButton)
            {
                if (elementslistviewCounter < ElementsListView.Items.Count - 6)
                {
                    UpButton.IsEnabled = true;
                    DownButton.IsEnabled = true;
                    ElementsListView.ScrollIntoView(ElementsListView.Items[elementslistviewCounter + 5]);
                    elementslistviewCounter += 5;
                }
                else
                {
                    UpButton.IsEnabled = true;
                    DownButton.IsEnabled = false;
                    ElementsListView.ScrollIntoView(ElementsListView.Items[ElementsListView.Items.Count - 1]);
                    elementslistviewCounter = ElementsListView.Items.Count - 5;
                }
            }
            else if (sender == UpButton)
            {
                if (elementslistviewCounter - 5 > 0)
                {
                    UpButton.IsEnabled = true;
                    DownButton.IsEnabled = true;
                    ElementsListView.ScrollIntoView(ElementsListView.Items[elementslistviewCounter - 5]);
                    elementslistviewCounter -= 5;
                }
                else
                {
                    UpButton.IsEnabled = false;
                    DownButton.IsEnabled = true;
                    ElementsListView.ScrollIntoView(ElementsListView.Items[0]);
                    elementslistviewCounter = 4;
                }
            }
        }

        private void MenuBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CloseButton) { this.Close(); }
            else if (sender == MaximizeButton)
            {
                if (this.WindowState != WindowState.Maximized) { this.WindowState = WindowState.Maximized; }
                else { this.WindowState = WindowState.Normal; }
            }
            else if (sender == MinimizeButton) { this.WindowState = WindowState.Minimized; }
        }

        private void EjectButton_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = (Popup)ElementsListView.FindResource("ElementsListViewPopup");
            popup.PlacementTarget = ElementsListView;
            popup.IsOpen = true;
        }

        private void ElementsPopup_Closed(object sender, EventArgs e)
        {
            if (ElementsListView.SelectedIndex <= 4)
            { elementslistviewCounter = 4; ElementsListView.ScrollIntoView(ElementsListView.Items[0]); }
            else if (ElementsListView.SelectedIndex <= ElementsListView.Items.Count - 6)
            { elementslistviewCounter = ElementsListView.SelectedIndex; ElementsListView.ScrollIntoView(ElementsListView.SelectedItem); }
            else { elementslistviewCounter = ElementsListView.Items.Count - 5; ElementsListView.ScrollIntoView(ElementsListView.Items[ElementsListView.Items.Count - 1]); }
            ElementsListView_SelectionChanged(ElementsListView, null);
        }
    }
}
