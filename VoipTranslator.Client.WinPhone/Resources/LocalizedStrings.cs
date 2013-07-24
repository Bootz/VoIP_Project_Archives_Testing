namespace VoipTranslator.Client.WinPhone.Resources
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
// ReSharper disable InconsistentNaming
        private static readonly AppResources _localizedResources = new AppResources();
// ReSharper restore InconsistentNaming

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}