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

        public async Task<ServiceResponse<Category>> CreateCategory(CreateCategoryDTO categoryData)
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

        public async Task<ServiceResponse<List<CategoryResponseDTO>>> GetAllCategories()
        {
            List<CategoryResponseDTO> categories = await _context.Categories
                .Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Books = c.Books.Select(b => new BookInCategoryDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        Content = b.Content,
                        CoverLink = b.CoverLink,
                        Url = b.Url
                    }).ToList()
                })
                .ToListAsync();

            ServiceResponse<List<CategoryResponseDTO>> response = new ServiceResponse<List<CategoryResponseDTO>>();

            response.Success = true;
            response.Data = categories;

            return response;
        }

        public async Task<ServiceResponse<CategoryResponseDTO>> GetCategoryById(int id)
        {
            ServiceResponse<CategoryResponseDTO> response = new ServiceResponse<CategoryResponseDTO>();

            Category category = await _context.Categories
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            CategoryResponseDTO crDTO = new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Books = category.Books.Select(b => new BookInCategoryDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Content = b.Content,
                    CoverLink = b.CoverLink,
                    Url = b.Url
                }).ToList()
            };

            if (category == null)
            {
                response.Success = false;
                response.Message = "Category not found.";
                return response;
            }

            response.Success = true;
            response.Data = crDTO;

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
