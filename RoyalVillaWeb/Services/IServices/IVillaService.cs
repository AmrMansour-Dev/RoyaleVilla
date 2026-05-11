using RoyalVilla.DTO;

namespace RoyalVillaWeb.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int ID);
        Task<T> CreateAsync<T>(VillaCreateDTO villaCreateDTO);
        Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO);
        Task<T> DeleteAsync<T>(int ID);
    }
}
