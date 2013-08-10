using System;
using System.Linq.Expressions;

namespace VoipTranslator.Server.Domain.Seedwork.Specifications
{
    /// <summary>
    /// A Direct Specification is a simple implementation
    /// of specification that acquire this from a lambda expression
    /// in  constructor
    /// </summary>
    /// <typeparam name="TEntity">Type of entity that check this specification</typeparam>
    public sealed class DirectSpecification<TEntity>
        : ISpecification<TEntity>
        where TEntity : class
    {
        #region Members

        readonly Func<TEntity, bool> _matchingCriteria;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for Direct Specification
        /// </summary>
        /// <param name="matchingCriteria">A Matching Criteria</param>
        public DirectSpecification(Func<TEntity, bool> matchingCriteria)
        {
            if (matchingCriteria == null)
                throw new ArgumentNullException("matchingCriteria");

            _matchingCriteria = matchingCriteria;
        }

        #endregion

        #region Override

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Func<TEntity, bool> SatisfiedBy()
        {
            return _matchingCriteria;
        }

        #endregion
    }
}
