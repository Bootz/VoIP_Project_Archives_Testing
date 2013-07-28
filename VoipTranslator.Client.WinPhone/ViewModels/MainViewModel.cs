using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using VoipTranslator.Client.Core;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        private readonly ApplicationManager _appManager;
        private string _number;

        public MainViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
            _appManager.StartApp();
        }

        public string Number
        {
            get { return _number; }
            set { SetProperty(ref _number, value); }
        }

        public ICommand KeyCommand
        {
            get { return new RelayCommand<string>(KeyPressed); }
        }

        public ICommand ClearCommand
        {
            get { return new RelayCommand(ClearPressed); }
        }

        public ICommand BackspaceCommand
        {
            get { return new RelayCommand(BackspacePressed); }
        }

        public ICommand CallCommand
        {
            get { return new RelayCommand(OnCall); }
        }

        private void OnCall()
        {
            //TODO: validate
            IsBusy = !IsBusy;

            //IsBusy = false;
        }

        private void BackspacePressed()
        {
            if (Number.Length > 0)
                Number = Number.Remove(Number.Length - 1);
        }

        private void ClearPressed()
        {
            Number = string.Empty;
        }

        private void KeyPressed(string key)
        {
            Number += key;
        }
    }
}