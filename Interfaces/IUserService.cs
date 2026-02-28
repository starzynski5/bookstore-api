using BookStoreAPI.DTOs;
using BookStoreAPI.Models;

namespace BookStoreAPI.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> CreateUser(User userData);

        Task<List<User>> GetAllUsers();

        Task<ServiceResponse<User>> GetUserById(int id);

        Task<ServiceResponse<User>> UpdateUser(int id, User updatedUserData);

        Task<ServiceResponse<User>> DeleteUser(int id);

        Task<ServiceResponse<string>> SubscribeToBook(int userId, int bookId);

        Task<ServiceResponse<List<Book>>> GetAllSubscribedBook(int userId);
    }
}
