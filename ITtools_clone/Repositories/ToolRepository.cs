using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ITtools_clone.Models;
using Microsoft.Extensions.Configuration;

namespace ITtools_clone.Repositories
{
    public interface IToolRepository
    {
        List<Tool> GetAllTools();
    }

    public class ToolRepository : IToolRepository
    {
        private readonly string _connectionString;

        public ToolRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public List<Tool> GetAllTools()
        {
            var tools = new List<Tool>();

            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = new MySqlCommand("SELECT * FROM tools", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tools.Add(new Tool
                {
                    tid = reader.GetInt32("tid"),
                    tool_name = reader.GetString("tool_name"),
                    description = reader.GetString("description"),
                    enabled = reader.GetBoolean("enabled"),
                    premium_required = reader.GetBoolean("premium_required"),
                    category_name = reader.GetString("category_name")
                });
            }
            return tools;
        }
    }
}
