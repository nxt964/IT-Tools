using System.Collections.Generic;
using ITtools_clone.Models;
using ITtools_clone.Repositories;

namespace ITtools_clone.Services
{
    public interface IUserService
    {
        List<User> GetAllUsers();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
    }
}
