namespace RoyalVillaWeb.Models
{
    public class ApiRequest
    {
        public SD.ApiType ApiType { get; set; } = SD.ApiType.Get;
        public string? Url { get; set; }
        public object? Data { get; set; }
        public string? Token { get; set; }

    }
}
