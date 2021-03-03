﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundLevelCalculator.Controls
{
    public class CommandComboBox : ComboBox
    {
        public CommandComboBox()
        {
            base.SelectionChanged += CommandComboBox_SelectionChanged;
        }

        private void CommandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandComboBox control = (CommandComboBox)sender;
            if (control != null && control.Command != null)
            {
                ICommand command = control.Command;
                if (command.CanExecute(control.CommandParameter)) { command.Execute(control.CommandParameter); }
            }
        }

        public static readonly DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandComboBox));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
                    DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandComboBox));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
