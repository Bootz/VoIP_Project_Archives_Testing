using Microsoft.Phone.Networking.Voip;

namespace VoipTranslator.Client.WinPhone.Agents
{
    /// <summary>
    /// An agent that is launched when the first call becomes active and is canceled when the last call ends.
    /// </summary>
    public class CallInProgressAgentImpl : VoipCallInProgressAgent
    {
        /// <summary>
        /// The first call has become active.
        /// </summary>
        protected override void OnFirstCallStarting()
        {
        }

        /// <summary>
        /// The last call has ended.
        /// </summary>
        protected override void OnCancel()
        {
            base.NotifyComplete();
        }
    }
}
