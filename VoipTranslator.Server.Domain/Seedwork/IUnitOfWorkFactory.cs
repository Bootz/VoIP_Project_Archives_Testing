namespace VoipTranslator.Server.Domain.Seedwork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}