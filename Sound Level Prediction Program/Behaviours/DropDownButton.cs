using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;

namespace Sound_Level_Prediction_Program
{
    public class DropDownButton : Behavior<Button>
    {
        private bool isContextMenuOpen;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
            AssociatedObject.AddHandler(Button.MouseRightButtonUpEvent, new RoutedEventHandler(AssociatedObject_MouseRightButtonDown), true);
        }

        private void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            if ((Button)sender != null && ((Button)sender).ContextMenu != null)
            {
                if (isContextMenuOpen == false)
                {
                    ((Button)sender).ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed), true);
                    ((Button)sender).ContextMenu.PlacementTarget = ((Button)sender);
                    ((Button)sender).ContextMenu.Placement = PlacementMode.Bottom;
                    ((Button)sender).ContextMenu.IsOpen = true;
                    isContextMenuOpen = true;
                }
            }
        }

        private void AssociatedObject_MouseRightButtonDown(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
            AssociatedObject.RemoveHandler(Button.MouseRightButtonUpEvent, new RoutedEventHandler(AssociatedObject_MouseRightButtonDown));
        }

        void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            isContextMenuOpen = false;

            if ((ContextMenu)sender != null)
            {
                ((ContextMenu)sender).RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed));
            }
        }
    }
}
