﻿using Autofac;
using VoipTranslator.Client.Core;
using VoipTranslator.Client.Core.Common;
using VoipTranslator.Client.Core.Compasition;

namespace VoipTranslator.Client.WinPhone
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

            var modules = new LayerModule[]
                {
                    new ApplicationModule(),
                    new CoreModule(),
                };

            modules.ForEach(builder.RegisterModule);

            var container = builder.Build();
            ServiceLocator.Init(container);
            modules.ForEach(m => m.OnPostContainerBuild(container));
        }
    }
}