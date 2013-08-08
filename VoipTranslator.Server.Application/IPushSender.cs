namespace VoipTranslator.Server.Application
{
    public interface IPushSender
    {
        void SendVoipPush(string pushUrl, string callerNumber, string callerName);
    }
}
