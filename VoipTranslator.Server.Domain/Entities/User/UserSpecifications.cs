using VoipTranslator.Server.Domain.Seedwork.Specifications;

namespace VoipTranslator.Server.Domain.Entities.User
{
    public static class UserSpecifications
    {
        public static Specification<User> UserId(string userId)
        {
            return new DirectSpecification<User>(user => user.UserId == userId);
        }

        public static Specification<User> Number(string number)
        {
            return new DirectSpecification<User>(user => user.Number == number);
        }
    }
}
