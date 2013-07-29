using Autofac;
using VoipTranslator.Protocol;
using VoipTranslator.Protocol.Serializers;

namespace VoipTranslator.Server
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().AsSelf().SingleInstance();
            builder.RegisterType<VoiceManager>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionsManager>().AsSelf().SingleInstance();
            builder.RegisterType<CommandBuilder>().AsSelf().SingleInstance();
            builder.RegisterType<CommandSerializer>().As<ICommandSerializer>().SingleInstance();
            builder.RegisterType<XmlDtoSerializer>().As<IDtoSerializer>().SingleInstance();
            base.Load(builder);
        }
    }
}
