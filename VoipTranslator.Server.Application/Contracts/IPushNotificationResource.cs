namespace VoipTranslator.Server.Application.Contracts
{
    public interface IPushNotificationResource
    {
        void SendVoipPush(string pushUrl, string callerNumber, string callerName);
    }
}
