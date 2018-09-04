using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Studio.Interfaces;

namespace Warewolf.Studio.Core
{
    public class HelpSetter : Behavior<FrameworkElement>
    {
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HelpSetter), new PropertyMetadata(null));

        public IUpdatesHelp DataContext
        {
            get => (IUpdatesHelp)GetValue(DataContextProperty);
            set => SetValue(DataContextProperty, value);
        }

        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register("DataContext", typeof(IUpdatesHelp), typeof(HelpSetter), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            FocusManager.AddGotFocusHandler(AssociatedObject, OnGotFocus);
        }

        void OnGotFocus(object sender, RoutedEventArgs args)
        {
            var mainViewModel = CustomContainer.Get<IShellViewModel>();
            DataContext?.UpdateHelpDescriptor(Text, mainViewModel);
        }
    }
}