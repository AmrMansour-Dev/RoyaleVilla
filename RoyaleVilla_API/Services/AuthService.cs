using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoyaleVilla_API.Data;
using RoyaleVilla_API.Models;
using RoyalVilla.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RoyaleVilla_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDBContext _DB;
        private readonly IMapper _Mapper;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDBContext DB, IMapper Mapper, IConfiguration configuration)
        {
            _DB = DB;
            _Mapper = Mapper;
            _configuration = configuration;
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _DB.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _DB.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.EmailAddress.ToLower());

                if (user == null || user.Password != loginRequestDTO.Password)
                {
                    return null;
                }

                //Generate token
                var token = GenerateJWTToken(user);

                return new LoginResponseDTO()
                {
                    UserDTO = _Mapper.Map<UserDTO>(user),
                    Token = token
                };

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occured while user trying to login", ex);
            }
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

        private string GenerateJWTToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWT")["Secret"]);

            var JWTdescriptor = new SecurityTokenDescriptor() // here we define the recipe of the token : claims,expiration,signingcredentials
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var tokenhandler = new JwtSecurityTokenHandler();

            var token = tokenhandler.CreateToken(JWTdescriptor);

            return tokenhandler.WriteToken(token); // here we make it compace (header.payload.signature)
        }
    }
}
