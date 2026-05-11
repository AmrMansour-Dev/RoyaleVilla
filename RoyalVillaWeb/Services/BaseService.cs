using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Text.Json;

namespace RoyalVillaWeb.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClient;
        public ApiResponse<object> ResponseModel { get; set; }

        private static readonly JsonSerializerOptions jsonSerializerOptions = new() // used to Convert from object to json
        {
            PropertyNameCaseInsensitive = true,
        };
        public BaseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            ResponseModel = new ApiResponse<object>();
        }

        public Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            throw new NotImplementedException();
        }
    }
}
