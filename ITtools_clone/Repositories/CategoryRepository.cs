using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ITtools_clone.Models;
using Microsoft.Extensions.Configuration;

namespace ITtools_clone.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM categories", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32("category_id"),
                    Name = reader.GetString("category_name")
                });
            }
            return categories;
        }
    }
}
