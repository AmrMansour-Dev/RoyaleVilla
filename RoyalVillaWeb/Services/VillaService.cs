using RoyalVilla.DTO;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class VillaService : IVillaService
    {
        public Task<T> CreateAsync<T>(VillaCreateDTO villaCreateDTO, string Token)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteAsync<T>(int ID, string Token)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAllAsync<T>(string Token)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(int ID, string Token)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO, string Token)
        {
            throw new NotImplementedException();
        }
    }
}
