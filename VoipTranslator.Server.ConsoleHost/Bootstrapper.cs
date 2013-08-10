using System.Collections.Generic;
using Autofac;
using VoipTranslator.Infrastructure;
using VoipTranslator.Server.Infrastructure;

namespace VoipTranslator.Server.Application.ConsoleHost
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
                    new ApplicationModule(),
                };

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            var container = builder.Build();
            ServiceLocator.Init(container);
            container.Resolve<AccountService>();
            container.Resolve<VoiceManager>();
        }
    }
}
