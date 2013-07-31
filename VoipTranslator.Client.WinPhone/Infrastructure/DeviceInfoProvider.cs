using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Info;
using Microsoft.Phone.Notification;
using VoipTranslator.Client.Core.Contracts;
using VoipTranslator.Protocol;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        private string _savedDeviceName = string.Empty;
        public const string PushChannelName = "VoipTranslatorChannel";

        public Task<string> GetPushUri()
        {
            throw new NotImplementedException();
        }

        private Task<string> InitPushChannel()
        {
            var taskSource = new TaskCompletionSource<string>();
            var pushChannel = HttpNotificationChannel.Find(PushChannelName);
            if (pushChannel == null)
            {
                pushChannel = new HttpNotificationChannel(PushChannelName);
                pushChannel.Open();
                pushChannel.BindToShellTile();
                pushChannel.BindToShellToast();
            }
            else
            {
                taskSource.TrySetResult(pushChannel.ChannelUri.ToStringIfNotNull());
            }
            pushChannel.ChannelUriUpdated += (s, e) => taskSource.TrySetResult(e.ChannelUri.ToStringIfNotNull());
            pushChannel.ErrorOccurred += (s, e) => taskSource.TrySetResult("");
            return taskSource.Task;
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
