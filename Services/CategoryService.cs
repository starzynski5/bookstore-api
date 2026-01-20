using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Interfaces;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BooksStoreDbContext _context;

        public CategoryService(BooksStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Category>> CreateCategory(Category categoryData)
        {
            ServiceResponse<Category> response = new ServiceResponse<Category>();
            
            bool isExistingCategory = await _context.Categories
                .Where(c => c.Name == categoryData.Name)
                .AnyAsync();

            if (isExistingCategory)
            {
                response.Success = false;
                response.Message = "Category with the same name already exists.";

                return response;
            }

            Category newCategory = new Category
            {
                Name = categoryData.Name
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            response.Data = newCategory;
            response.Message = "Category created successfully.";
            response.Success = true;

            return response;
        }

        public async Task<ServiceResponse<List<Category>>> GetAllCategories()
        {
            List<Category> categories = await _context.Categories.ToListAsync();

            ServiceResponse<List<Category>> response = new ServiceResponse<List<Category>>();

            response.Success = true;
            response.Data = categories;

            return response;
        }

        public async Task<ServiceResponse<Category>> GetCategoryById(int id)
        {
            ServiceResponse<Category> response = new ServiceResponse<Category>();
            
            Category category = await _context.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found.";
                return response;
            }

            response.Success = true;
            response.Data = category;

            return response;
        }

        public async Task<ServiceResponse<Category>> UpdateCategory(Category updatedCategory, int id)
        {
            ServiceResponse<Category> response = new ServiceResponse<Category>();

            Category existingCategory = await _context.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (existingCategory == null)
            {
                response.Success = false;
                response.Message = "Category with this id was not found.";

                return response;
            }

            existingCategory.Name = updatedCategory.Name;

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Category updated.";

            return response;
        }

        public async Task<ServiceResponse<Category>> DeleteCategory(int id)
        {
            ServiceResponse<Category> response = new ServiceResponse<Category>();

            Category category = await _context.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category with this id was not found.";

                return response;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Category removed successfully.";

            return response;
        }
    }
}
