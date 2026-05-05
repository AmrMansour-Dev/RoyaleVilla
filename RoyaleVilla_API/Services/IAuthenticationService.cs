using RoyaleVilla_API.Models.DTO;

namespace RoyaleVilla_API.Services
{
    public interface IAuthenticationService
    {
        Task<UserDTO> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);
    }
}
