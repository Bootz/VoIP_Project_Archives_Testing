using VoipTranslator.Client.Core.Common;

namespace VoipTranslator.Client.Core.Logging
{
    public static class LogFactory
    {
        public static ILogger GetLogger<T>()
        {
            return ServiceLocator.ResolveWith<ILogger>(typeof (T).Name);
        }
    }
}
