using Autofac;
using VoipTranslator.Protocol.Commands;
using VoipTranslator.Protocol.Serializers;
using VoipTranslator.Protocol.Serializers.Builtin;

namespace VoipTranslator.Server.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().AsSelf().SingleInstance();
            builder.RegisterType<VoiceManager>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionsService>().AsSelf().SingleInstance();
            builder.RegisterType<CommandBuilder>().AsSelf().SingleInstance();
            builder.RegisterType<CommandSerializer>().As<ICommandSerializer>().SingleInstance();
            builder.RegisterType<XmlDtoSerializer>().As<IDtoSerializer>().SingleInstance();
            base.Load(builder);
        }
    }
}
