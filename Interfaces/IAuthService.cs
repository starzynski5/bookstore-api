using BookStoreAPI.DTOs;

namespace BookStoreAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(RegisterRequestDTO request);

        Task<ServiceResponse<string>> Login(LoginRequestDTO request);
    }
}
