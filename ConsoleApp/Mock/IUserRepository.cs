namespace ConsoleApp.Mock
{
    public interface IUserRepository
    {
        User? GetById(int id);
    }
}
