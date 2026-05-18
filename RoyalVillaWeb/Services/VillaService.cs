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

        public Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO)
        {
            return SendAsync<T> (new ApiRequest()
            {
                ApiType = SD.ApiType.Post,
                Url = $"{ApiEndPoint}",
                Data = villaCreateDTO
            });
        }

        public Task<T?> DeleteAsync<T>(int ID)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Delete,
                Url = $"{ApiEndPoint}/{ID}"
            });
        }

        public Task<T?> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Get,
                Url = $"{ApiEndPoint}"
            });
        }

        public Task<T?> GetAsync<T>(int ID)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Get,
                Url = $"{ApiEndPoint}/{ID}"
            });
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.Put,
                Url = $"{ApiEndPoint}/{villaUpdateDTO.Id}",
                Data = villaUpdateDTO
            });
        }
    }
}
