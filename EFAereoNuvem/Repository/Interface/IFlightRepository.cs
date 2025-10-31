using EFAereoNuvem.Models;

namespace EFAereoNuvem.Repository.Interface;
public interface IFlightRepository
{
    Task CreateAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    Task DeleteAsync(Guid Id);
    Task AddAsync(Flight flight);
    Task<Flight> GetByIdAsync(Guid Id);
    Task<IEnumerable<Flight>> GetAllAsync();

    //métodos adicionais específicos para o domínio
    Task<IEnumerable<Flight>> GetByRouteAsync(string origin, string destination);
    Task<IEnumerable<Flight>> GetByDateAsync(DateTime dateInitial); 
    Task<IEnumerable<Flight>> GetAvailableFlightsAsync(string origin, string destination, DateTime date);
    Task<IEnumerable<Flight>> GetDirectFlightsAsync();
    Task<IEnumerable<Flight>> GetByRouteDirectFlightsAsync(string origin, string destination);
    Task<Flight?> GetByCode(string codeFlight);
    Task<Flight> GetByIdWithScales(Guid id); 
}