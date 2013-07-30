using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CyclopsToolkit.WinPhone.Behaviours
{
    public class EnumToVisualStateBehavior : Behavior<FrameworkElement>
    {
        public object EnumObject
        {
            get { return (object)GetValue(EnumObjectProperty); }
            set { SetValue(EnumObjectProperty, value); }
        }

        public static readonly DependencyProperty EnumObjectProperty =
            DependencyProperty.Register("EnumObject", typeof(object), typeof(EnumToVisualStateBehavior), new PropertyMetadata(null, EnumObjectStaticChanged));

        private static void EnumObjectStaticChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var parent = ((EnumToVisualStateBehavior)d).AssociatedObject;
                if (parent is Control)
                {
                    var state = e.NewValue.ToString();
                    VisualStateManager.GoToState(parent as Control, state, true);
                }
            }
        }
    }
}
