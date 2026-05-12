using RoyalVilla.DTO;

namespace RoyalVillaWeb.Services.IServices
{
    public interface IVillaService
    {
        Task<T?> GetAllAsync<T>(string Token);
        Task<T?> GetAsync<T>(int ID,string Token);
        Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string Token);
        Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string Token);
        Task<T?> DeleteAsync<T>(int ID, string Token);
    }
}
