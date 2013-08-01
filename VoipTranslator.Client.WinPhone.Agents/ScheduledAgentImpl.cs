using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Phone.Networking.Voip;
using Microsoft.Phone.Scheduler;
using Windows.ApplicationModel;
using Windows.Phone.Networking.Voip;
using VoipTranslator.Protocol.Dto;

namespace VoipTranslator.Client.WinPhone.Agents
{
    public class ScheduledAgentImpl : ScheduledTaskAgent
    {
        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        protected override void OnInvoke(ScheduledTask task)
        {
            var incomingCallTask = task as VoipHttpIncomingCallTask;
            if (incomingCallTask != null)
            {
                // Parse the the incoming push notification payload
                IncomingCallInfoDto pushNotification;
                using (var ms = new MemoryStream(incomingCallTask.MessageBody))
                {
                    var xs = new XmlSerializer(typeof(IncomingCallInfoDto));
                    pushNotification = (IncomingCallInfoDto)xs.Deserialize(ms);
                }

                String defaultContactImageUri = Package.Current.InstalledLocation.Path + @"\Assets\DefaultContactImage.jpg";
                String logoUrl = Package.Current.InstalledLocation.Path + @"\Assets\ApplicationIcon.png";

                VoipPhoneCall callObj;
                var callCoordinator = VoipCallCoordinator.GetDefault();
                callCoordinator.RequestNewIncomingCall("/Views/SplashScreenPage.xaml?incomingCall=" + pushNotification.Number,
                                                       pushNotification.Name, pushNotification.Number, new Uri(defaultContactImageUri),
                                                       "VoipTranslator.Client.WinPhone", new Uri(defaultContactImageUri), "VoIP Translator", new Uri(logoUrl), VoipCallMedia.Audio,
                                                       TimeSpan.FromMinutes(5), out callObj);
                callObj.AnswerRequested += callObj_AnswerRequested;
                callObj.RejectRequested += callObj_RejectRequested;
            }
            else
            {
                var keepAliveTask = task as VoipKeepAliveTask;
                if (keepAliveTask != null)
                {
                    this.Complete();
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unknown scheduled task type {0}", task.GetType()));
                }
            }
        }

        void callObj_RejectRequested(VoipPhoneCall sender, CallRejectEventArgs args)
        {
            sender.NotifyCallHeld();
        }

        async void callObj_AnswerRequested(VoipPhoneCall sender, CallAnswerEventArgs args)
        {
            sender.NotifyCallActive();
            await Task.Delay(3000);
            sender.NotifyCallEnded();
        }

        // This is a request to complete this agent
        protected override void OnCancel()
        {
            this.Complete();
        }

        // Complete this agent.
        private void Complete()
        {
            base.NotifyComplete();
        }
    }
}
