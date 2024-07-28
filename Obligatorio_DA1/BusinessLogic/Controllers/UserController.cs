using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using Domain;
using Persistence;

namespace BusinessLogic.Controllers
{
    public class UserController
    {
        private const string AddUserMessage = "User correctly registered.";
        private readonly IRepository<User> _userRepository;

        public UserController()
        {
        }

        public UserController(IRepository<User> aUserRepository)
        {
            _userRepository = aUserRepository;
        }

        public string Register(User aUser, string passwordConfirmation)
        {
            UserValidator.IsValid(aUser, passwordConfirmation);
            if (IsEmailInUse(aUser.Email))
            {
                throw new BusinessLogicException("The email is already in use.");
            }
            AdminUser(aUser);
            _userRepository.Add(aUser);
            return AddUserMessage;
        }
    
        public virtual User AuthenticateUser(string email, string password)
        {
            var user = _userRepository.Find(u => u.Email == email);
            if (user == null || user.Password != password)
            {
                throw new BusinessLogicException("The email and/or password are incorrect.");
            }
            return user;
        }
    
        private void AdminUser(User aUser)
        {
            aUser.IsAdmin = _userRepository.FindAll().Count == 0;
        }
    
        public bool IsEmailInUse(string email)
        {
            return _userRepository.Find(u => u.Email == email) != null;
        }
    }
}
