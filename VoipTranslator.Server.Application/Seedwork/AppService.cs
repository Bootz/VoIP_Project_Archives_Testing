using VoipTranslator.Server.Domain.Seedwork;

namespace VoipTranslator.Server.Application.Seedwork
{
    public abstract class AppService
    {
        protected ITransactionFactory TransactionFactory { get; set; }
        //public IUnitOfWorkFactory UnitOfWorkFactory { get; set; }

        protected AppService(ITransactionFactory transactionFactory)
        {
            TransactionFactory = transactionFactory;
        }
    }
}
