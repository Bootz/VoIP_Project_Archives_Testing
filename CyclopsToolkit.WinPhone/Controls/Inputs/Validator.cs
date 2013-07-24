using System;

namespace CyclopsToolkit.WinPhone.Controls.Inputs
{
    public class Validator
    {
        public Validator(Func<string, string> validatorFunc)
        {
            ValidatorFunc = validatorFunc;
        }

        public Validator()
        {
        }

        public Func<string, string> ValidatorFunc { get; set; }

        public event EventHandler<ValidatingEventArgs> ManualValidating = delegate { }; 

        public bool Validate()
        {
            var args = new ValidatingEventArgs();
            ManualValidating(this, args);
            return args.ValidationSuccess;
        }
    }

    public class ValidatingEventArgs : EventArgs
    {
        public bool ValidationSuccess { get; set; }
    }

    public class ValidatableTextBoxDesignData
    {
        public string Text
        {
            get { return "Some design data text"; }
        }

        public string Header
        {
            get { return "Some header:"; }
        }
    }
}