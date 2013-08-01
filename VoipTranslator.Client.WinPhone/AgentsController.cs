using System;
using System.Threading.Tasks;
using Microsoft.Phone.Networking.Voip;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Scheduler;

namespace VoipTranslator.Client.WinPhone
{
    public class AgentsController
    {
        private const string IncomingCallTaskName = "VoipTranslator.Client.WinPhone.IncomingCallTask";
        private const string KeepAliveTaskName = "VoipTranslator.Client.WinPhone.KeepAliveTask";
        public const string AppPushChannel = "VoipTranslator.Client.WinPhone.AppChannel2";
        private bool _agentsAreInited = false;

        public async Task<string> InitAndGetPushUri()
        {
            var pushUri = await GetPushUri();
            if (!_agentsAreInited)
            {
                _agentsAreInited = true;
                InitHttpNotificationTask();
                InitKeepAliveTask();
            }
            return pushUri;
        }

        private Task<string> GetPushUri()
        {
            var taskSource = new TaskCompletionSource<string>();
            var httpChannel = HttpNotificationChannel.Find(AppPushChannel);
            if (httpChannel == null)
            {
                httpChannel = new HttpNotificationChannel(AppPushChannel);
                httpChannel.ChannelUriUpdated += (s, e) => taskSource.TrySetResult(e.ChannelUri.ToString());
                httpChannel.ErrorOccurred += (s, e) => taskSource.TrySetResult(string.Empty);
                httpChannel.Open();
            }
            else
            {
                taskSource.TrySetResult(httpChannel.ChannelUri.ToString());
            }
            return taskSource.Task;
        }

        private void InitHttpNotificationTask()
        {
            // Obtain a reference to the existing task, if any.
            var incomingCallTask = ScheduledActionService.Find(IncomingCallTaskName) as VoipHttpIncomingCallTask;
            if (incomingCallTask != null)
            {
                if (incomingCallTask.IsScheduled == false)
                {
                    // The incoming call task has been unscheduled due to OOM or throwing an unhandled exception twice in a row
                    ScheduledActionService.Remove(IncomingCallTaskName);
                }
                else
                {
                    // The incoming call task has been scheduled and is still scheduled so there is nothing more to do
                    return;
                }
            }

            // Create a new incoming call task.
            incomingCallTask = new VoipHttpIncomingCallTask(IncomingCallTaskName, AppPushChannel);
            incomingCallTask.Description = "Incoming call task";
            ScheduledActionService.Add(incomingCallTask);
        }

        private void InitKeepAliveTask()
        {
            // Obtain a reference to the existing task, if any.
            var keepAliveTask = ScheduledActionService.Find(KeepAliveTaskName) as VoipKeepAliveTask;
            if (keepAliveTask != null)
            {
                if (keepAliveTask.IsScheduled == false)
                {
                    // The keep-alive task has been unscheduled due to OOM or throwing an unhandled exception twice in a row
                    ScheduledActionService.Remove(KeepAliveTaskName);
                }
                else
                {
                    // The keep-alive task has been scheduled and is still scheduled so there is nothing more to do
                    return;
                }
            }

            keepAliveTask = new VoipKeepAliveTask(KeepAliveTaskName);
            keepAliveTask.Interval = new TimeSpan(12, 0, 0); // Every 12 hours
            keepAliveTask.Description = "keep-alive task";
            ScheduledActionService.Add(keepAliveTask);
        }
    }
}
