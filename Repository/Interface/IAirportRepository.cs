using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface;
public interface IAirportRepository
{
    Task CreateAsync(Airport airport);
    Task UpdateAsync(Airport airport);
    Task DeleteAsync(Guid id);
    Task<Airport?> GetByIdAsync(Guid id);
    Task<Airport?> GetByIATA(string IATA);
    Task<List<Airport>> GetAll(int pageNumber, int pageSize);
}
