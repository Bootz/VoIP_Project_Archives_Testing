using System.Collections.Generic;
using System.Linq;
using VoipTranslator.Server.Domain.Entities.User;
using VoipTranslator.Server.Domain.Seedwork.Specifications;

namespace VoipTranslator.Server.Infrastructure.Persistence
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new List<User>(100);

        public void Add(User user)
        {
            lock (_users)
            {
                _users.Add(user);
            }
        }

        public IEnumerable<User> AllMatching(ISpecification<User> specification)
        {
            lock (_users)
            {
                return _users.Where(specification.SatisfiedBy());
            }
        }

        public User FirstMatching(ISpecification<User> specification)
        {
            return AllMatching(specification).FirstOrDefault();
        }
    }
}
