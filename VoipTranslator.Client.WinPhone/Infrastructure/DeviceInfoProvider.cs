using System;
using System.Threading.Tasks;
using Microsoft.Phone.Info;
using VoipTranslator.Client.Core.Contracts;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        private readonly AgentsController _agentsController;
        private string _savedDeviceName = string.Empty;

        public DeviceInfoProvider(AgentsController agentsController)
        {
            _agentsController = agentsController;
        }

        public Task<string> GetPushUri()
        {
            return _agentsController.InitAndGetPushUri();
        }

        public async Task<string> GetDeviceName()
        {
            try
            {
                if (!string.IsNullOrEmpty(_savedDeviceName))
                    return _savedDeviceName;
                const string manufacturerProperty = "DeviceManufacturer";
                var model = DeviceStatus.DeviceName;
                var manufacturer = string.Empty;
                object output;
                if (DeviceExtendedProperties.TryGetValue(manufacturerProperty, out output))
                    manufacturer = output.ToString();
                return _savedDeviceName = string.Format("{0} {1}", manufacturer, model);
            }
            catch (Exception ex)
            {
                return "Unknown";
            }
        }

        public async Task<string> GetOsName()
        {
            return Environment.OSVersion.ToString();
        }
    }
}
