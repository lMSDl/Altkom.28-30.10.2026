namespace TDD.Mock
{
    public interface IUserRepository
    {
        bool Exists(string email);
        void Add(User user);
    }
}
