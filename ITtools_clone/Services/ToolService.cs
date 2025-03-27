using System.Collections.Generic;
using ITtools_clone.Models;
using ITtools_clone.Repositories;
using System.Linq;

namespace ITtools_clone.Services
{
    public interface IToolService
    {
        List<Tool> GetAllTools();
        List<Tool> GetEnabledTools();
        Dictionary<string, List<string>> GetCategorizedTools();
        Tool GetToolById(int id);
        void AddTool(Tool tool);
        void UpdateTool(Tool tool);
        void DeleteTool(int id);
        bool ValidateTool(Tool tool);
    }

    public class ToolService : IToolService
    {
        private readonly IToolRepository _toolRepository;

        public ToolService(IToolRepository toolRepository)
        {
            _toolRepository = toolRepository;
        }

        public List<Tool> GetAllTools()
        {
            return _toolRepository.GetAllTools();
        }

        public List<Tool> GetEnabledTools()
        {
            return _toolRepository.GetAllTools().Where(t => t.enabled).ToList();
        }

        public Dictionary<string, List<string>> GetCategorizedTools()
        {
            var enabledTools = GetEnabledTools();
            
            return enabledTools
                .Where(t => !string.IsNullOrEmpty(t.category_name))
                .GroupBy(t => t.category_name!)
                .ToDictionary(g => g.Key, g => g.Select(t => t.tool_name!).ToList());
        }

        public Tool GetToolById(int id)
        {
            return _toolRepository.GetToolById(id);
        }

        public void AddTool(Tool tool)
        {
            // Add business logic/validation before adding
            if (ValidateTool(tool))
            {
                _toolRepository.AddTool(tool);
            }
            else
            {
                throw new System.Exception("Tool validation failed");
            }
        }

        public void UpdateTool(Tool tool)
        {
            if (ValidateTool(tool))
            {
                _toolRepository.UpdateTool(tool);
            }
            else
            {
                throw new System.Exception("Tool validation failed");
            }
        }

        public void DeleteTool(int id)
        {
            _toolRepository.DeleteTool(id);
        }

        public bool ValidateTool(Tool tool)
        {
            // Add business validation logic here
            if (string.IsNullOrEmpty(tool.tool_name))
                return false;

            if (string.IsNullOrEmpty(tool.description))
                return false;

            return true;
        }
    }
}
