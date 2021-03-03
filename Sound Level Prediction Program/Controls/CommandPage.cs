using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SoundLevelCalculator.Controls
{
    public class CommandPage : Page
    {
        public CommandPage()
        {
            base.PreviewKeyDown += OnPreviewKeyDown;
        }

        public static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { e.Handled = true; }

            CommandPage control = (CommandPage)sender;
            if (control != null && control.Command != null)
            {
                ICommand command = control.Command;
                if (command.CanExecute(control.CommandParameter)) { command.Execute(control.CommandParameter ?? e.Key); }
            }
        }

        public static readonly DependencyProperty CommandProperty =
                    DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandPage));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
                    DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandPage));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
