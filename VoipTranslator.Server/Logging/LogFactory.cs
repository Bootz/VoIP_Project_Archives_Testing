namespace VoipTranslator.Server.Logging
{
    public static class LogFactory
    {
        public static ILogger GetLogger<T>()
        {
            return ServiceLocator.ResolveWith<ILogger>(typeof (T).Name);
        }
    }
}
