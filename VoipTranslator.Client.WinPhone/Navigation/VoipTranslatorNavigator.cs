using System;
using System.Windows.Navigation;
using CyclopsToolkit.WinPhone.Navigation;
using VoipTranslator.Client.Core;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.WinPhone.ViewModels;
using VoipTranslator.Client.WinPhone.Views;
using VoipTranslator.Infrastructure;

namespace VoipTranslator.Client.WinPhone.Navigation
{
    internal class VoipTranslatorNavigator : NavigationManagerBase
    {
        private readonly ApplicationManager _appManager;
        private readonly AccountManager _accountManager;

        public const string IncomingCallArgument = "incomingCall";

        public VoipTranslatorNavigator(NavigationBuilder navigationBuilder, 
            ApplicationManager appManager,
            AccountManager accountManager) 
            : base(navigationBuilder)
        {
            _appManager = appManager;
            _accountManager = accountManager;
            _appManager.RegistrationRequested += _appManager_OnRegistrationRequested;
        }

        private void _appManager_OnRegistrationRequested(object sender, EventArgs e)
        {
            ServiceLocator.Resolve<RegistrationViewModel>().Show();
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
                string number;
                if (navigationContext.QueryString.TryGetValue(IncomingCallArgument, out number) && !string.IsNullOrWhiteSpace(number))
                {
                    ServiceLocator.Resolve<MainViewModel>().ShowAndCall(number);
                }
                else
                {
                    ServiceLocator.Resolve<MainViewModel>().Show();
                }
            }
        }
    }
}
