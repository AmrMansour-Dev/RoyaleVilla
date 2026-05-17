using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private const string ApiEndPoint = "/api/villa";

        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
        }
        public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Post,
                Url = $"{ApiEndPoint}"+"/login",
                Data = loginRequestDTO
            });
        }

        public Task<T?> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Post,
                Url = $"{ApiEndPoint}" + "/register",
                Data = registerationRequestDTO
            });
        }
    }
}
