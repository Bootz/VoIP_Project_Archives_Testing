using VoipTranslator.Server.Entities;

namespace VoipTranslator.Server.Interfaces
{
    public interface IUsersRepository
    {
        void Add(User user);
        bool Exists(string userId);
        User GetById(string userId);
    }
}
