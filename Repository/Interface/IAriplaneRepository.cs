using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface;
public interface IAirplaneRepository
{
    Task CreateAsync(Airplane airplane);
    Task UpdateAsync(Airplane airplane);
    Task DeleteAsync(Guid id);
    Task<Airplane?> GetById(Guid id);
    Task<Airplane?> GetByPrefix(string prefix);
    Task<List<Airplane>> GetAll(int pageNumber, int pageSize);
    Task<bool> PrefixExists(string prefix);
    Task<List<Flight>> GetFlightScheduleAsync(Guid airplaneId);
}