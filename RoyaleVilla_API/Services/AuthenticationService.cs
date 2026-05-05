using RoyaleVilla_API.Models.DTO;

namespace RoyaleVilla_API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<bool> IsEmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
