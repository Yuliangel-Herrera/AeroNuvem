using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface
{
    public interface IAirplaneRepository
    {
        Task Create(Airplane airplane);
        Task Update(Airplane airplane);
        Task Delete(Guid id);
        Task<Airplane> GetById(Guid id);
        Task<Airplane?> GetByPrefix(string prefix);
        Task<List<Airplane>> GetAll();
        Task<bool> PrefixExists(string prefix);
    }
}