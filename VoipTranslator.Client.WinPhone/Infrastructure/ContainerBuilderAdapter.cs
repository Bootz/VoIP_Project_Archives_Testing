using System;
using Autofac;
using CyclopsToolkit.WinPhone.Contracts;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class ContainerBuilderAdapter : IIoCContainerBuilder
    {
        private readonly ContainerBuilder _container;

        public ContainerBuilderAdapter(ContainerBuilder container)
        {
            _container = container;
        }

        public void RegisterAsSingleton(Type timplementation, Type tinterface)
        {
            _container.RegisterType(timplementation).As(tinterface).SingleInstance();
        }

        public void Register(Type timplementation, Type tinterface)
        {
            _container.RegisterType(timplementation).As(tinterface).InstancePerDependency();
        }

        public void RegisterAsSingleton(Type timplementation)
        {
            _container.RegisterType(timplementation).AsSelf().SingleInstance();
        }

        public void Register(Type timplementation)
        {
            _container.RegisterType(timplementation).AsSelf().InstancePerDependency();
        }
    }
}
