using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using VoipTranslator.Client.Core;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class CallViewModel : ViewModelBaseEx
    {
        private CallState _state;
        private readonly CallsManager _callsManager;
        private string _number;

        public CallViewModel(CallsManager callsManager)
        {
            _callsManager = callsManager;
            _callsManager.CallEnded += (s,e) => Dispatcher.BeginInvoke(_callsManager_OnCallEnded);
            _callsManager.IncomingCall += (s, e) => Dispatcher.BeginInvoke(_callsManager_OnIncomingCall);
        }

        private void _callsManager_OnIncomingCall()
        {
            State = CallState.Ringing;
        }

        private void _callsManager_OnCallEnded()
        {
            State = CallState.None;
        }

        public CallState State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public string Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        public ICommand EndCallCommand
        {
            get { return new RelayCommand(EndCall); }
        }

        public ICommand DeclineCallCommand
        {
            get { return new RelayCommand(DeclineCall); }
        }

        public ICommand AnswerCallCommand
        {
            get { return new RelayCommand(AnswerCall); }
        }

        public ICommand CallCommand
        {
            get { return new RelayCommand<string>(Call); }
        }

        public async void Call(string number)
        {
            Number = number;
            State = CallState.InCall;
            var answered = await _callsManager.Dial(number);
            if (!answered)
                State = CallState.None;
        }

        private void EndCall()
        {
            State = CallState.None;
            _callsManager.End();
        }

        private void DeclineCall()
        {
            State = CallState.None;
            _callsManager.Decline();
        }

        private void AnswerCall()
        {
            State = CallState.InCall;
            _callsManager.Answer();
        }
    }

    public enum CallState
    {
        None,
        Ringing,
        InCall,
    }
}
