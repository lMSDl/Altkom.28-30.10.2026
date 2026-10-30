namespace TDD.Mock
{
    public class User
    {
        public string Email { get; }
        public string PasswordHash { get; }

        public User(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}