using SoundLevelCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Xml;

namespace SoundLevelCalculator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static ToggleButton tempToggleButton = null;

        private MainWindowViewModel viewModel = new MainWindowViewModel();

        private string selectedTreeViewItem = string.Empty;

        private string previewTreeViewItem = string.Empty;

        private Thickness? treeViewMargin = null;

        private string[] nonExpandableElements = new string[] 
        {
            "Kanał prosty",
            "Wentylator",
            "Kratka wyciągowa",
            "Kratka nawiewna",
            "Przepustnica",
            "Skrzynka rozprężna",
            "Trójnik",
            "Czwórnik",
            "Tłumik",
            "Łuk",
            "Kolano",
            "Redukcja"
        };

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            this.Loaded += MainWindow_Loaded;
            ElementsTypeComboBox.SelectionChanged += CommandComboBox_SelectionChanged;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SharedFunctions.TryFindAndExpandNode(ElementsTreeView, StaticResources.ElementsList.Document.FirstChild.ChildNodes, "Kanał prosty");
        }

        private void MenuBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CloseButton) { this.Close(); }
            else if (sender == MaximizeButton)
            {
                if (this.WindowState != WindowState.Maximized)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
            }
            else if (sender == MinimizeButton)
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            tempToggleButton = btn;
            Popup popup = (from FrameworkElement fwe in btn.Resources.Values
                           where fwe is Popup select fwe).FirstOrDefault() as Popup;
            popup.PlacementTarget = btn;
            popup.IsOpen = true;
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            Popup btn = sender as Popup;

            if (tempToggleButton != null)
            {
                tempToggleButton.IsChecked = false;
            }
        }

        private void CommandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string itemName = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
            SharedFunctions.TryFindAndExpandNode(ElementsTreeView, StaticResources.ElementsList.Document.FirstChild.ChildNodes, itemName);
        }

        private void ContentGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = SharedFunctions.FindVisualChildren<TextBlock>(sender as FrameworkElement).FirstOrDefault();
            ToggleButton toggleButton = SharedFunctions.FindVisualChildren<ToggleButton>(sender as FrameworkElement).FirstOrDefault();

            if (!nonExpandableElements.Contains(textBlock.Text))
            {
                e.Handled = true;

                if (toggleButton.IsMouseOver)
                {
                    TreeViewItem tvi = SharedFunctions.FindVisualParent<TreeViewItem>(sender as FrameworkElement);

                    if (tvi.IsExpanded)
                    {
                        tvi.IsExpanded = false;
                    }
                    else
                    {
                        int counter = 0;
                        tvi.IsExpanded = true;
                        IEnumerable<TextBlock> valuesList = SharedFunctions.FindVisualChildren<TextBlock>(tvi as FrameworkElement);

                        foreach (TextBlock tb in valuesList)
                        {
                            if (tb.Text == previewTreeViewItem)
                            {
                                break;
                            }
                            else
                            {
                                counter++;
                            }
                        }
                        
                        if (counter != valuesList.Count())
                        {
                            tvi = SharedFunctions.FindVisualParent<TreeViewItem>(valuesList.ToList()[counter]);
                            tvi.IsSelected = true;
                        }
                    }
                }
            }
        }

        private void ElementsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            previewTreeViewItem = selectedTreeViewItem;
            selectedTreeViewItem = (e.NewValue as XmlElement).SelectSingleNode("Head").InnerText;

            if (nonExpandableElements.Contains(selectedTreeViewItem))
            {
                ItemTextBlock.Text = selectedTreeViewItem;

                foreach (ComboBoxItem comboBoxItem in ElementsTypeComboBox.Items)
                {
                    if (comboBoxItem.Content.ToString() == selectedTreeViewItem)
                    {
                        comboBoxItem.IsSelected = true;
                        break;
                    }
                }
            }
        }

        private void ElementsTreeView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollViewer scrollViewer = SharedFunctions.FindVisualChildren<ScrollViewer>(sender as FrameworkElement).FirstOrDefault();
            treeViewMargin = treeViewMargin ?? ((FrameworkElement)sender).Margin;

            if (scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            {
                ((FrameworkElement)sender).Margin = new Thickness(0);
            }
            else
            {
                ((FrameworkElement)sender).Margin = (Thickness)treeViewMargin;
            }
        }
    }
}
