using System;
using System.Collections.Generic;
using System.Linq;
using VoipTranslator.Server.Entities;
using VoipTranslator.Server.Interfaces;

namespace VoipTranslator.Server.Infrastructure
{
    public class UsersRepository : IUsersRepository
    {
        private readonly List<User> _users = new List<User>(100);
        
        public void Add(User user)
        {
            lock (_users)
            {
                _users.Add(user);
            }
        }

        public bool Exists(string userId)
        {
            lock (_users)
            {
                return _users.Any(i => i.UserId == userId);
            }
        }

        public User GetById(string userId)
        {
            lock (_users)
            {
                return _users.FirstOrDefault(i => i.UserId == userId);
            }
        }
    }
}
