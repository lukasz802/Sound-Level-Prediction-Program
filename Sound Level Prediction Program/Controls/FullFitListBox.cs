using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sound_Level_Prediction_Program.Controls
{
    public class FullFitListBox : ListBox
    {
        private List<double> heightList;
        private double totalHeight;
        private FrameworkElement container;

        public FullFitListBox()
        {
            heightList = new List<double>();
            totalHeight = 0;
            ((INotifyCollectionChanged)this.Items).CollectionChanged += OnItemsCountChanged;
        }

        private void OnItemsCountChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Items.Count > 0)
            {
                container = ItemContainerGenerator.ContainerFromIndex(0) as FrameworkElement;
                container.SizeChanged += Container_SizeChanged;
            }
        }

        private void Container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement temp = null;
            heightList.Add(e.NewSize.Height);

            if (heightList.Count > 4) { heightList.RemoveAt(0); }

            totalHeight = heightList.Sum();
            if (totalHeight > this.Height * 1.11)
            {
                temp = ItemContainerGenerator.ContainerFromIndex(3) as FrameworkElement;
                if (temp != null)
                {
                    (FindVisualChildren<TextBlock>(temp).ToList()[1] as TextBlock).Visibility = Visibility.Hidden;
                }
                else
                {
                    temp = ItemContainerGenerator.ContainerFromIndex(1) as FrameworkElement;
                    (FindVisualChildren<TextBlock>(temp).ToList()[1] as TextBlock).Visibility = Visibility.Hidden;
                }
            }
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
