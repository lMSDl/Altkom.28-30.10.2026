namespace ConsoleApp.Mock
{
    public class AccessService
    {
        private readonly IUserRepository _userRepository;

        public AccessService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool CanAccessAdminPanel(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
                return false;

            if (!user.IsActive)
                return false;

            if (user.Role != "Admin")
                return false;

            return true;
        }
    }
}
