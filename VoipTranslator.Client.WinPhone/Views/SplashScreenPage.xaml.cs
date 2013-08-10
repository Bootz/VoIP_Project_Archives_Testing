using System.Windows.Navigation;
using CyclopsToolkit.WinPhone.Navigation;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Infrastructure;

namespace VoipTranslator.Client.WinPhone.Views
{
    public partial class SplashScreenPage : NavigatablePage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ServiceLocator.Resolve<NavigationManagerBase>().StartupNavigation(NavigationContext);
        }
    }
}