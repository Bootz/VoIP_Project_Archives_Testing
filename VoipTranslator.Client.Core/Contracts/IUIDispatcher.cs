using System;
using System.Threading.Tasks;

namespace VoipTranslator.Client.Core.Contracts
{
    public interface IUIDispatcher 
    {
        void ToUIThread(Action action);

        Task ToUIThreadTask(Action action);

        void ToUIThreadIfNeeded(Action action);

        bool IsUIThread();

        void ToUIThreadSync(Action action);
    }
}
