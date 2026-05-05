using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models;
using RoyaleVilla_API.Models.DTO;

namespace RoyaleVilla_API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDBContext _DB;
        private readonly IMapper _Mapper;

        public AuthenticationService(ApplicationDBContext DB, IMapper Mapper)
        {
            _DB = DB;
            _Mapper = Mapper;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _DB.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (await IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    throw new InvalidOperationException($"User With email '{registerationRequestDTO.Email}' Already Exists!");
                }

                User user = new User()
                {
                    Email = registerationRequestDTO.Email,
                    CreatedDate = DateTime.Now,
                    Name = registerationRequestDTO.Name,
                    Password = registerationRequestDTO.Password,
                    Role = string.IsNullOrEmpty(registerationRequestDTO.Role) ? "Customer" : registerationRequestDTO.Role
                };
                await _DB.Users.AddAsync(user);
                await _DB.SaveChangesAsync();

                return _Mapper.Map<UserDTO>(user); //Here We return UserDTO For Security Purposes and not expose every property.
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occured while user registeration", ex);
            }

        }
    }
}
