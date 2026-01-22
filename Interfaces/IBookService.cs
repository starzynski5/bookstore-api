using BookStoreAPI.DTOs;
using BookStoreAPI.Models;

namespace BookStoreAPI.Interfaces
{
    public interface IBookService
    {
        Task<ServiceResponse<List<Book>>> GetAllBooks();

        Task<ServiceResponse<Book>> GetBookById(int id);

        Task<ServiceResponse<BookResponseDto>> CreateBook(CreateBookDTO book);

        Task<ServiceResponse<Book>> UpdateBook(CreateBookDTO updatedBook, int id);

        Task<ServiceResponse<Book>> DeleteBook(int id);
    }
}
