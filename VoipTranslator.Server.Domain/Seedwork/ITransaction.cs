using System;

namespace VoipTranslator.Server.Domain.Seedwork
{
    public interface ITransaction : IDisposable
    {
        void Complete();
    }
}