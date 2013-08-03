using System;

namespace VoipTranslator.Server.Domain.Seedwork
{
    /// <summary>
    /// Base interface for implement a "Repository Pattern"
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}


