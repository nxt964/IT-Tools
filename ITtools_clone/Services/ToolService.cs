using System.Collections.Generic;
using ITtools_clone.Models;
using ITtools_clone.Repositories;

namespace ITtools_clone.Services
{
    public interface IToolService
    {
        List<Tool> GetAllTools();
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
    }
}
