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

        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var Client = _httpClient.CreateClient("RoualVillaAPI");

                var message = new HttpRequestMessage()
                {
                    Method = GetMethodType(apiRequest.ApiType),
                    RequestUri = new Uri(apiRequest.Url,uriKind:UriKind.Relative)
                };
                if(apiRequest.Data != null )
                {
                    message.Content = JsonContent.Create(apiRequest.Data, options: jsonSerializerOptions); // Serialize object to JSON HTTP content
                }

                var apiResponse = await Client.SendAsync(message);

                return await apiResponse.Content.ReadFromJsonAsync<T>(jsonSerializerOptions); //Deserialize JSON response body to C# object of type T
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return default;
            }
        }

        public static HttpMethod GetMethodType(SD.ApiType apiType)
        {
            switch(apiType)
            {
                case SD.ApiType.Delete:
                    return HttpMethod.Delete;
                case SD.ApiType.Post:
                    return HttpMethod.Post;
                case SD.ApiType.Put:
                    return HttpMethod.Put;
                default:
                    return HttpMethod.Get;

            }
        }
    }
}
