using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using static SoundLevelCalculator.Controls.NumericUpDown;

namespace SoundLevelCalculator
{
    public static class SharedFunctions
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
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

        public static T FindVisualParent<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(depObj);

                if (parent == null)
                {
                    return null;
                }
                else if (parent is T)
                {
                    return (T)parent;
                }
                else
                {
                    return FindVisualParent<T>(parent);
                }
            }
            else
            {
                return null;
            }
        }

        public static void TryFindAndExpandNode(ItemsControl itemsControl, XmlNodeList xmlSchema, string nodeName)
        {
            TreeViewItem tviItem = null;

            foreach (XmlNode node in xmlSchema)
            {
                if (VerifyIfNodeExists(node, nodeName))
                {
                    tviItem = itemsControl.ItemContainerGenerator.ContainerFromItem(node) as TreeViewItem;
                    tviItem.IsExpanded = true;
                    tviItem.UpdateLayout();

                    if (node.SelectSingleNode("Head").InnerText.ToString() == nodeName)
                    {
                        tviItem.IsSelected = true;
                        break;
                    }
                    else
                    {
                        TryFindAndExpandNode(tviItem, node.SelectNodes("ListViewItem"), nodeName);
                    }

                    return;
                }
                else
                {
                    continue;
                }
            }
        }

        public static object ConvertUnits(object inputData, Units inputUnit, Units outputUnit)
        {
            if (inputUnit == Units.CubicDecimetersPerSecond)
            {
                if (outputUnit == Units.CubicMetersPerHour)
                {
                    return Convert.ToInt32(Math.Round(Convert.ToDouble(inputData) * 3.6, 0, MidpointRounding.AwayFromZero));
                }
                else if (outputUnit == Units.CubicDecimetersPerSecond)
                {
                    return Convert.ToInt32(inputData);
                }
            }
            else if (inputUnit == Units.CubicMetersPerHour)
            {
                if (outputUnit == Units.CubicDecimetersPerSecond)
                {
                    return Math.Round(Convert.ToDouble(inputData) / 3.6, 0, MidpointRounding.AwayFromZero);
                }
                else if (outputUnit == Units.CubicMetersPerHour)
                {
                    return Math.Round(Convert.ToDouble(inputData), 0, MidpointRounding.AwayFromZero);
                }
            }
            else if (inputUnit == Units.Meters)
            {
                if (outputUnit == Units.Milimeters)
                {
                    return Convert.ToInt32(Math.Round(Convert.ToDouble(inputData) * 1000, 0, MidpointRounding.AwayFromZero));
                }
                else if (outputUnit == Units.Meters)
                {
                    return Math.Round(Convert.ToDouble(inputData), 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                if (outputUnit == Units.Meters)
                {
                    return Math.Round(Convert.ToDouble(inputData) / 1000, 2, MidpointRounding.AwayFromZero);
                }
                else if (outputUnit == Units.Milimeters)
                {
                    return Convert.ToInt32(inputData);
                }
            }

            throw new Exception($"Could not convert from {inputUnit} to {outputUnit}.");
        }

        private static bool VerifyIfNodeExists(XmlNode xmlNode, string nodeName)
        {
            bool result = false;

            if (xmlNode.HasChildNodes && xmlNode.SelectSingleNode("Head") != null)
            {
                if (xmlNode.SelectSingleNode("Head").InnerText.ToString() == nodeName)
                {
                    return true;
                }

                foreach (XmlNode node in xmlNode.SelectNodes("ListViewItem"))
                {
                    if (node.SelectSingleNode("Head").InnerText.ToString() == nodeName)
                    {
                        result = true;
                    }
                    else
                    {
                        result = VerifyIfNodeExists(node, nodeName);
                    }

                    if (result)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}
