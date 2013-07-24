using Autofac;
using VoipTranslator.Client.Core.Compasition;

namespace VoipTranslator.Client.Core
{
    public class CoreModule : LayerModule
    {
        protected override void OnMap(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<AccountManager>().AsSelf().SingleInstance();
            base.OnMap(builder);
        }
    }
}
