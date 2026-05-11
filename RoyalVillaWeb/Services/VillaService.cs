using RoyalVilla.DTO;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Services
{
    public class VillaService : IVillaService
    {
        public Task<T> CreateAsync<T>(VillaCreateDTO villaCreateDTO)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteAsync<T>(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(int ID)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
