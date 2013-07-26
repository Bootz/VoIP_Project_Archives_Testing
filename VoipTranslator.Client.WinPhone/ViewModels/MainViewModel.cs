using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoipTranslator.Client.Core;

namespace VoipTranslator.Client.WinPhone.ViewModels
{
    public class MainViewModel : ViewModelBaseEx
    {
        private readonly ApplicationManager _appManager;

        public MainViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
            _appManager.StartApp();
        }
    }
}
