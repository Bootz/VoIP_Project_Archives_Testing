using System.Collections.Generic;
using VoipTranslator.Server.Domain.Seedwork.Specifications;

namespace VoipTranslator.Server.Domain.Entities.User
{
    public interface IUserRepository
    {
        void Add(User user);

        IEnumerable<User> AllMatching(ISpecification<User> specification);

        User FirstMatching(ISpecification<User> specification);
    }
}
