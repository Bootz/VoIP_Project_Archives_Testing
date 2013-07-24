using System;
using System.Threading.Tasks;
using System.Windows;
using VoipTranslator.Client.Core.Contracts;

namespace VoipTranslator.Client.WinPhone.Infrastructure
{
    public class Dispatcher : IUIDispatcher
    {
        public void ToUIThread(Action action)
        {
            Deployment.Current.Dispatcher.BeginInvoke(action);
        }

        public Task ToUIThreadTask(Action action)
        {
            throw new NotImplementedException();
        }

        public void ToUIThreadIfNeeded(Action action)
        {
            if (IsUIThread())
                ToUIThread(action);
            else
                action();
        }

        public bool IsUIThread()
        {
            return Deployment.Current.Dispatcher.CheckAccess();
        }

        public void ToUIThreadSync(Action action)
        {
            throw new NotImplementedException();
        }
    }
}
