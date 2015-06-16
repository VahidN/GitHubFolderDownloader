using System.Windows;
using System.Windows.Controls;

namespace GitHubFolderDownloader.Behaviors
{
    public class ScrollToEndBehavior
    {
        public static readonly DependencyProperty OnTextChangedProperty =
                    DependencyProperty.RegisterAttached(
                    "OnTextChanged",
                    typeof(bool),
                    typeof(ScrollToEndBehavior),
                    new UIPropertyMetadata(false, onTextChanged)
                    );

        public static bool GetOnTextChanged(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(OnTextChangedProperty);
        }

        public static void SetOnTextChanged(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(OnTextChangedProperty, value);
        }

        private static void onTextChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var textBox = dependencyObject as TextBox;
            var newValue = (bool)e.NewValue;

            if (textBox == null || (bool)e.OldValue == newValue)
            {
                return;
            }

            TextChangedEventHandler handler = (sender, args) =>
                ((TextBox)sender).ScrollToEnd();

            if (newValue)
            {
                textBox.TextChanged += handler;
            }
            else
            {
                textBox.TextChanged -= handler;
            }
        }
    }
}