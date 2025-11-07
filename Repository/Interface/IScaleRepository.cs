using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface
{
    public interface IScaleRepository
    {
        Task Create(Scale scale);
        Task Update(Scale scale);
        Task Delete(Guid id);
        Task DeleteByFlightId(Guid flightId);
        Task<Scale> GetById(Guid id);
        Task<List<Scale>> GetByFlightId(Guid flightId);
        Task<List<Scale>> GetAll();
    }
}