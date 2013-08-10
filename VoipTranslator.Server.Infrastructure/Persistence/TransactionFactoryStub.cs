using VoipTranslator.Server.Domain.Seedwork;

namespace VoipTranslator.Server.Infrastructure.Persistence
{
    public class TransactionFactoryStub : ITransactionFactory
    {
        public ITransaction Create()
        {
            return new TransactionStub();
        }
    }

    public class TransactionStub : ITransaction
    {
        public void Dispose()
        {
        }

        public void Complete()
        {
        }
    }
}
