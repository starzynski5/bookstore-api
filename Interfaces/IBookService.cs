using BookStoreAPI.DTOs;
using BookStoreAPI.Models;

namespace BookStoreAPI.Interfaces
{
    public interface IBookService
    {
        Task<ServiceResponse<List<Book>>> GetAllBooks();

        Task<ServiceResponse<Book>> GetBookById(int id);
    }
}
