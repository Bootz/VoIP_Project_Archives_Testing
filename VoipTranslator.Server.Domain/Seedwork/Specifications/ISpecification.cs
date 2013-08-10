using System;

namespace VoipTranslator.Server.Domain.Seedwork.Specifications
{
    public interface ISpecification<in TEntity>
    {
        Func<TEntity, bool> SatisfiedBy();
    }
}