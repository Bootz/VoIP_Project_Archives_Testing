using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace CyclopsToolkit.WinPhone.Navigation
{
    public abstract class NavigationManagerBase
    {
        private readonly NavigationBuilder _navigationBuilder;
        private const string ViewModelIdArgumentName = "viewModelId";

        protected NavigationManagerBase(NavigationBuilder navigationBuilder)
        {
            _navigationBuilder = navigationBuilder;
        }

        public Dictionary<Type, Type> ViewsMap { get { return _navigationBuilder.ViewsMap; } }

        public abstract Type[] RootViewModels { get; }

        public abstract void StartupNavigation(NavigationContext navigationContext);

        public void Navigate<T>(T viewModel)
        {
            var page = ViewsMap[viewModel.GetType()];

            var frame = Application.Current.RootVisual as PhoneApplicationFrame;
            if (frame == null)
                throw new InvalidOperationException("Frame is null");

            if (frame.Content is FrameworkElement)
            {
                var dc = ((FrameworkElement) frame.Content).DataContext;
                if (dc != null && dc.Equals(viewModel))
                    return;
            }

            bool isRootPage = RootViewModels.Contains(viewModel.GetType());
            if (isRootPage)
            {
                while (frame.BackStack.Any())
                {
                    var entry = frame.RemoveBackEntry();
                    //TODO: log entry.Source;
                }
            }

            bool navigationResult = frame.Navigate(GetPageUri(page, ViewModelsRegistry.Register(viewModel)));
            if (!navigationResult)
            {
                //TODO: log and do smth
            }
        }

        public static object GetDataContext(NavigationContext navContext)
        {
            string id;
            if (navContext != null && navContext.QueryString != null &&
                navContext.QueryString.TryGetValue(ViewModelIdArgumentName, out id))
            {
                return ViewModelsRegistry.GetById(id);
            }
            return null;
        }

        private Uri GetPageUri(Type page, string viewModelId)
        {
            return new Uri(string.Format("/Views/{0}.xaml?{1}={2}", page.Name, ViewModelIdArgumentName, viewModelId), UriKind.Relative);
        }
    }
}
