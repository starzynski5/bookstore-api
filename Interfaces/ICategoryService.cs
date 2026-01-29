using BookStoreAPI.DTOs;
using BookStoreAPI.Models;

namespace BookStoreAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResponse<Category>> CreateCategory(CreateCategoryDTO categoryData);

        Task<ServiceResponse<List<Category>>> GetAllCategories();

        Task<ServiceResponse<Category>> GetCategoryById(int id);

        Task<ServiceResponse<Category>> UpdateCategory(Category updatedCategory, int id);

        Task<ServiceResponse<Category>> DeleteCategory(int id);
    }
}
