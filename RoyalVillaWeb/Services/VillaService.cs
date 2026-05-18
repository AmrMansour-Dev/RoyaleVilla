using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private const string ApiEndPoint = "/api/villa";

        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClient,httpContextAccessor)
        {
        }

        public Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string Token)
        {
            return SendAsync<T> (new ApiRequest()
            {
                ApiType = SD.ApiType.Post,
                Url = $"{ApiEndPoint}",
                Token = Token,
                Data = villaCreateDTO
            });
        }

        public Task<T?> DeleteAsync<T>(int ID, string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Delete,
                Url = $"{ApiEndPoint}/{ID}",
                Token = Token
            });
        }

        public Task<T?> GetAllAsync<T>(string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Get,
                Url = $"{ApiEndPoint}",
                Token = Token
            });
        }

        public Task<T?> GetAsync<T>(int ID, string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Get,
                Url = $"{ApiEndPoint}/{ID}",
                Token = Token
            });
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Put,
                Url = $"{ApiEndPoint}/{villaUpdateDTO.Id}",
                Token = Token,
                Data = villaUpdateDTO
            });
        }
    }
}
