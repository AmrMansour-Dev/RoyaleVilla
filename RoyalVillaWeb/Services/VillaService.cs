using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly string _VillaUrl;
        private const string ApiEndPoint = "/api/villa";

        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _VillaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string Token)
        {
            return SendAsync<T> (new ApiRequest()
            {
                ApiType = SD.ApiType.Post,
                Url = $"{_VillaUrl}{ApiEndPoint}",
                Token = Token,
                Data = villaCreateDTO
            });
        }

        public Task<T?> DeleteAsync<T>(int ID, string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Delete,
                Url = $"{_VillaUrl}{ApiEndPoint}/{ID}",
                Token = Token
            });
        }

        public Task<T?> GetAllAsync<T>(string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Get,
                Url = $"{_VillaUrl}{ApiEndPoint}",
                Token = Token
            });
        }

        public Task<T?> GetAsync<T>(int ID, string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Get,
                Url = $"{_VillaUrl}{ApiEndPoint}/{ID}",
                Token = Token
            });
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string Token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Put,
                Url = $"{_VillaUrl}{ApiEndPoint}/{villaUpdateDTO.Id}",
                Token = Token,
                Data = villaUpdateDTO
            });
        }
    }
}
