using Microsoft.Phone.Networking.Voip;

namespace VoipTranslator.Client.WinPhone.Agents
{
    /// <summary>
    /// An agent that is invoked when the UI process calls Microsoft.Phone.Networking.Voip.VoipBackgroundProcess.Launched()
    /// and is canceled when the UI leaves the foreground.
    /// </summary>
    public sealed class ForegroundLifetimeAgentImpl : VoipForegroundLifetimeAgent
    {
        public static bool IsInForeground = false;
        /// <summary>
        /// A method that is called as a result of 
        /// </summary>
        protected override void OnLaunched()
        {
            IsInForeground = true;
        }

        protected override void OnCancel()
        {
            IsInForeground = false;
            base.NotifyComplete();
        }
    }
}
