using VoipTranslator.Client.Core;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        private readonly ApplicationManager _appManager;
        private readonly CallsManager _callsManager;
        private string _number;

        public MainViewModel(ApplicationManager appManager,
            CallsManager callsManager)
        {
            _appManager = appManager;
            _callsManager = callsManager;
        }

        public override void OnNavigatedTo()
        {
            _appManager.StartApp();
            base.OnNavigatedTo();
        }
    }
}