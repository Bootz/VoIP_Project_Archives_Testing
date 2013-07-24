using System.Windows;
using GalaSoft.MvvmLight;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class FrameViewModel : ViewModelBase
    {
        private bool _isBusy;

        public FrameViewModel()
        {
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                Deployment.Current.Dispatcher.BeginInvoke(() => Set("IsBusy", ref _isBusy, value));
            }
        }
    }
}
