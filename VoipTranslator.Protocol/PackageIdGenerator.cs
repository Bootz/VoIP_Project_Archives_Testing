using System.Threading;

namespace VoipTranslator.Protocol
{
    public static class PackageIdGenerator
    {
        private static long _lastUsed = 0;

        public static long Generate()
        {
            Interlocked.Increment(ref _lastUsed);
            return _lastUsed;
        }
    }
}
