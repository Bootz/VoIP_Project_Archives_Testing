using VoipTranslator.Client.Core;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        private readonly ApplicationManager _appManager;
        private readonly CallViewModel _callViewModel;
        private readonly AccountManager _accountManager;
        private string _number = null;

        public MainViewModel(ApplicationManager appManager,
            CallViewModel callViewModel,
            AccountManager accountManager)
        {
            _appManager = appManager;
            _callViewModel = callViewModel;
            _accountManager = accountManager;
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
        }

        public override async void Show()
        {
            IsBusy = true;
            await _appManager.StartApp();
            if (_number != null && _accountManager.IsRegistered)
            {
                _callViewModel.AnswerCall();
                _number = null;
            }
            IsBusy = false;
            base.Show();
        }

        public void ShowAndCall(string number)
        {
            _number = number;
            Show();
        }
    }
}