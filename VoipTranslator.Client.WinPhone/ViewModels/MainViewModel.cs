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
            _appManager.StartApp();
        }
    }
}