using System;
using System.Collections.Generic;
using System.Windows;
using CyclopsToolkit.WinPhone.Contracts;

namespace CyclopsToolkit.WinPhone.Navigation
{
    public class NavigationBuilder
    {
        private readonly IIoCContainerBuilder _containerBuilderBuilder;
        private readonly Dictionary<string, Type> _viewModelsAsStaticResources = new Dictionary<string, Type>();

        public NavigationBuilder(IIoCContainerBuilder containerBuilderBuilder)
        {
            _containerBuilderBuilder = containerBuilderBuilder;
            ViewsMap = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Collection of mapped ViewModel to View
        /// </summary>
        public Dictionary<Type, Type> ViewsMap { get; set; }

        /// <summary>
        /// Registers ViewModel
        /// </summary>
        public NavigationRegisteredViewModel<TViewModel> RegisterViewModel<TViewModel>() //where TViewModel : ViewModelBase
        {
            return new NavigationRegisteredViewModel<TViewModel>(this, _containerBuilderBuilder);
        }

        public void RegisterStaticViewModels(Func<Type, object> resolver)
        {
            foreach (var item in _viewModelsAsStaticResources)
            {
                if (!Application.Current.Resources.Contains(item.Key))
                    Application.Current.Resources.Add(item.Key, resolver(item.Value));
            }
        }

        internal void AddStaticViewModel<TViewModel>(string key = "")
        {
            var vmType = typeof (TViewModel);
            _viewModelsAsStaticResources[string.IsNullOrEmpty(key) ? vmType.Name : key] = vmType;
        }
    }
}
