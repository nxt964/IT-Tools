using System.Collections.Generic;
using ITtools_clone.Models;
using ITtools_clone.Repositories;


namespace ITtools_clone.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }
    }
}
