using System.Threading.Tasks;

namespace VoipTranslator.Server.Application.Contracts
{
    public interface IPushNotificationResource
    {
        Task<bool> SendVoipPush(string pushUrl, string callerNumber, string callerName);
    }
}
