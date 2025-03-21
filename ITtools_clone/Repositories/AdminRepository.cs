using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ITtools_clone.Models;
using Microsoft.Extensions.Configuration;

namespace ITtools_clone.Repositories
{
    public interface IAdminRepository
    {
        List<Admin> GetAllAdmins();
    }

    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public List<Admin> GetAllAdmins()
        {
            var admins = new List<Admin>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM admins", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                admins.Add(new Admin
                {
                    Id = reader.GetInt32("aid"),
                    AdminName = reader.GetString("admin_name")
                });
            }
            return admins;
        }
    }
}
