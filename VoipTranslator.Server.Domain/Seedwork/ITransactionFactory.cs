namespace VoipTranslator.Server.Domain.Seedwork
{
    public interface ITransactionFactory
    {
        ITransaction Create();
    }
}