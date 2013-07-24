using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyclopsToolkit.WinPhone.Controls.Inputs
{
    public partial class ValidatableTextBox : UserControl
    {
        public const string ValidationFailedStateName = "ValidationFailedState";
        public const string NormalStateName = "NormalState";

        public static readonly DependencyProperty ValidatorProperty =
            DependencyProperty.Register("Validator", typeof(Validator), typeof(ValidatableTextBox), new PropertyMetadata(null, ValidatorPropertyChangedStatic));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(ValidatableTextBox), new PropertyMetadata("", HeaderPropertyChangedStatic));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ValidatableTextBox), new PropertyMetadata(null, TextPropertyChangedStatic));

        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.Register("EnterCommand", typeof(ICommand), typeof(ValidatableTextBox), new PropertyMetadata(null));

        public static readonly DependencyProperty IsDigitProperty =
            DependencyProperty.Register("IsDigit", typeof(bool), typeof(ValidatableTextBox), new PropertyMetadata(false));

        public ValidatableTextBox()
        {
            InitializeComponent();
            errorTextBox.Text = string.Empty;
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Validator Validator
        {
            get { return (Validator)GetValue(ValidatorProperty); }
            set { SetValue(ValidatorProperty, value); }
        }

        public ICommand EnterCommand
        {
            get { return (ICommand)GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public bool IsDigit
        {
            get { return (bool)GetValue(IsDigitProperty); }
            set { SetValue(IsDigitProperty, value); }
        }

        private static void HeaderPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
            {
                if (e.NewValue != null &&
                    !string.IsNullOrWhiteSpace(e.NewValue.ToString()) &&
                    !e.NewValue.ToString().EndsWith(":"))
                    box.Header += ":";
            }
        }

        private static void TextPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
                box.TextPropertyChanged();
        }

        private static void ValidatorPropertyChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as ValidatableTextBox;
            if (box != null)
                box.ValidatorPropertyChanged();
        }

        private void ValidatorPropertyChanged()
        {
            if (Validator != null)
            {
                Validator.ManualValidating += ValidatorValidating;
            }
        }

        private void ValidatorValidating(object sender, ValidatingEventArgs e)
        {
            e.ValidationSuccess = Validate();
        }

        private void TextPropertyChanged()
        {
            Validate();
        }

        private bool Validate()
        {
            if (Validator == null)
                return true;
            Text = textBoxValue.Text;
            string error = Validator.ValidatorFunc(Text);
            errorTextBox.Text = error;
            bool errorEmpty = string.IsNullOrEmpty(error);

            errorBorder.Visibility = errorEmpty ? Visibility.Collapsed : Visibility.Visible;
            VisualStateManager.GoToState(this, errorEmpty ? NormalStateName : ValidationFailedStateName, true);
            return errorEmpty;
        }

        private void TextBoxValueKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                if (Validate())
                {
                    if (EnterCommand != null && EnterCommand.CanExecute(null))
                        EnterCommand.Execute(null);
                    hiddenButton.Focus(); // workaround to hide the keyboard
                }
            }
        }
    }
}
