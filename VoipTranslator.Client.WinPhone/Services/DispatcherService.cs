using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace VoipTranslator.Client.WinPhone.Services
{
    public class DispatcherService : IApplicationService
    {
        private readonly DispatcherTimer _dispatcherTimer;

        public DispatcherService()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromTicks(3000);
            _dispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
            FrameworkDispatcher.Update();
        }

        private void frameworkDispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Microphone requires this
            FrameworkDispatcher.Update();
        }

        void IApplicationService.StartService(ApplicationServiceContext context)
        {
            _dispatcherTimer.Start();
        }

        void IApplicationService.StopService()
        {
            _dispatcherTimer.Stop();
        }
    }
}
