namespace TDD.Mock
{
    public class UserRegistrationService
    {
        private IUserRepository _userRepository;
        private IPasswordHasher _passwordHasher;

        public UserRegistrationService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public void Register(string email, string password)
        {
            EnsureEmailIsUnique(email);

            var hashedPassword = _passwordHasher.Hash(password);
            var user = new User(email, hashedPassword);

            _userRepository.Add(user);
        }

        private void EnsureEmailIsUnique(string email)
        {
            if (_userRepository.Exists(email))
            {
                throw new InvalidOperationException();
            }
        }
    }
}
