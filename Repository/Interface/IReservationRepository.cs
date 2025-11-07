using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface;
public interface IReservationRepository
{
    Task CreateAsync(Reservation reservation);
    Task DeleteAsync(Guid Id);
    Task<Reservation?> GetByCode(string CodeRersevation);
    Task<Reservation?> GetByIdAsync(Guid Id);
    Task<IEnumerable<Reservation>?> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Reservation>> GetReservationsByFlightIdAsync(Guid flightId);
}
