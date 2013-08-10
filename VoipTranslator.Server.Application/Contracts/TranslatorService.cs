using System;

namespace VoipTranslator.Server.Application.Contracts
{
    public interface ITranslationResource
    {
        void AppendRawData(byte[] data, Action<byte[]> callback);
    }
}
