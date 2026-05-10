using RoyalVilla.DTO;

namespace RoyaleVilla_API.Services
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
