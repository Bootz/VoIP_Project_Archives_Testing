using Microsoft.Phone.Controls;

namespace CyclopsToolkit.WinPhone.Navigation
{
    public class NavigatablePage : PhoneApplicationPage
    {
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            DataContext = NavigationManagerBase.GetDataContext(NavigationContext);
            base.OnNavigatedTo(e);
        }
    }
}
