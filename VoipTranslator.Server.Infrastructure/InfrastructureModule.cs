using Autofac;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransportResource>().As<ITransportResource>().SingleInstance();
            base.Load(builder);
        }
    }
}
