using System;
using System.Windows.Navigation;
using CyclopsToolkit.WinPhone.Navigation;
using VoipTranslator.Client.Core;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.WinPhone.ViewModels;
using VoipTranslator.Client.WinPhone.Views;

namespace VoipTranslator.Client.WinPhone.Navigation
{
    class VoipTranslatorNavigator : NavigationManagerBase
    {
        private readonly AccountManager _accountManager;

        public VoipTranslatorNavigator(NavigationBuilder navigationBuilder, 
            AccountManager accountManager) 
            : base(navigationBuilder)
        {
            _accountManager = accountManager;
        }

        public override Type[] RootViewModels
        {
            get
            {
                return new[]
                    {
                        typeof (MainPage),
                        typeof (RegistrationPage),
                    };
            }
        }

        public override void StartupNavigation(NavigationContext navigationContext)
        {
            if (!_accountManager.IsRegistered)
            {
                ServiceLocator.Resolve<RegistrationViewModel>().Show();
            }
            else
            {
                ServiceLocator.Resolve<MainViewModel>().Show();
            }
        }
    }
}
