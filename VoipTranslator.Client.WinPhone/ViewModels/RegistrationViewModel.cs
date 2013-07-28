using System.Windows;
using System.Linq;
using System.Windows.Input;
using CyclopsToolkit.WinPhone.Controls.Inputs;
using VoipTranslator.Client.Core;
using VoipTranslator.Client.Core.Common;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class RegistrationViewModel : ViewModelBaseEx
    {
        private readonly AccountManager _accountManager;
        private string _number;
        private bool _isNumberInitiallyEmpty = true;

        public RegistrationViewModel(AccountManager accountManager)
        {
            _accountManager = accountManager;
            NumberValidator = new Validator(NumberValidation);
        }

        private string NumberValidation(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
                return "Number is empty";

            if (!arg.StartsWith("+"))
                return "Number should start with '+'";

            if (arg.Remove(0, 1).Any(i => !char.IsDigit(i)))
                return "Number contains invalid symbols";

            if (arg.Length < 3)
                return "Number too short";

            if (arg.Length > 20)
                return "Number too long";

            return string.Empty;
        }

        public string Number
        {
            get { return _number; }
            set
            {
                _isNumberInitiallyEmpty = false;
                SetProperty(ref _number, value);
                UpdateCanExecutes();
            }
        }

        public Validator NumberValidator { get; private set; }

        public ICommand RegisterCommand
        {
            get { return CreateCommand(Register, CanRegister); }
        }

        private async void Register()
        {
            IsBusy = true;
            var success = await _accountManager.Register(Number);
            if (!success)
            {
                MessageBox.Show("Invalid number. Please, specify your full number in the following format: +(country code)(service code)(number) (i.e. +375123456789)");
            }
            else
            {
                ServiceLocator.Resolve<MainViewModel>().Show();
            }
            IsBusy = false;
        }

        private bool CanRegister()
        {
            return !_isNumberInitiallyEmpty || NumberValidator.Validate();
        }
    }
}
