using System.Threading.Tasks;

namespace VoipTranslator.Client.Core.Contracts
{
    public interface IDeviceInfoProvider
    {
        Task<string> GetPushUri();

        Task<string> GetDeviceName();

        Task<string> GetOsName();
    }
}
