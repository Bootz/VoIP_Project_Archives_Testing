using System.Collections.Generic;
using Autofac;
using VoipTranslator.Server;
using VoipTranslator.Server.Infrastructure;
using VoipTranslator.Server.Interfaces;

namespace Voip.Server.ConsoleHost
{
    public static class Bootstrapper
    {
        private static bool _wasRun;

        public static void Run()
        {
            if (_wasRun)
                return;
            _wasRun = true;

            var builder = new ContainerBuilder();

            var modules = new List<Module>
                {
                    new InfrastructureModule(),
                    new CoreModule(),
                };

            modules.ForEach(builder.RegisterModule);

            var container = builder.Build();
            ServiceLocator.Init(container);
            container.Resolve<ITransportResource>();
        }
    }
}
