using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class FrameViewModel : ViewModelBase
    {
        private bool _isBusy;

        public FrameViewModel()
        {
        }

        public Dispatcher Dispatcher { get { return Deployment.Current.Dispatcher; } }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Dispatcher.BeginInvoke(() => Set("IsBusy", ref _isBusy, value)); }
        }
    }
}
