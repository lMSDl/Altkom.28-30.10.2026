namespace TDD.Mock
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
