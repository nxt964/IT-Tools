using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ITtools_clone.Models;
using Microsoft.Extensions.Configuration;

namespace ITtools_clone.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
    }

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM users", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    usid = reader.GetInt32("usid"),
                    username = reader.GetString("username"),
                    password = reader.GetString("password"),
                    email = reader.GetString("email"),
                    premium = reader.GetBoolean("premium"),
                    is_admin = reader.GetBoolean("is_admin")
                });
            }
            return users;
        }
    }
}
