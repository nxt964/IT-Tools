using System.Collections.Generic;
using ITtools_clone.Models;
using ITtools_clone.Repositories;

namespace ITtools_clone.Services
{
    public interface IAdminService
    {
        List<Admin> GetAllAdmins();
    }

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public List<Admin> GetAllAdmins()
        {
            return _adminRepository.GetAllAdmins();
        }
    }
}
