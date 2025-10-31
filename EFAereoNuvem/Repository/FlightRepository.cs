using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Repository;
public class FlightRepository(AppDBContext context) : IFlightRepository
{
    private readonly AppDBContext _context = context; 

    public async Task CreateAsync(Flight flight)
    {
        await _context.Flights.AddAsync(flight);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Flight flight)
    {
        _context.Flights.Update(flight);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddAsync(Flight flight)
    {
        await _context.Flights.AddAsync(flight);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Flight>> GetAllAsync()
    {
        return await _context.Flights
            .OrderBy(f => f.Departure)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Flight?> GetByIdAsync(Guid id)
    {
        var flight = await _context.Flights.FindAsync(id);
        return flight;
    }

    public async Task<IEnumerable<Flight>> GetByRouteAsync(string origin, string destination)
    {
        return await _context.Flights
            .Where(f => f.Origin == origin && f.Destination == destination)
            .OrderBy(f => f.Departure)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Flight>> GetByDateAsync(DateTime date)
    {
        return await _context.Flights
            .Where(f => f.Departure.Date == date.Date)
            .OrderBy(f => f.Departure)
            .AsNoTracking()
            .ToListAsync();
    }

    //consulta de voos disponiveis com base na origem, destino e data
    public async Task<IEnumerable<Flight>> GetAvailableFlightsAsync(string origin, string destination, DateTime date)
    {
        // Busca os voos com aeronave e reservas carregadas
        var flights = await _context.Flights
            .Include(f => f.Airplane)
                .ThenInclude(a => a.Armchairs)
            .Include(f => f.Reservations)
                .ThenInclude(r => r.ReservedArmchair)
            .Where(f => f.Origin == origin
                     && f.Destination == destination
                     && f.Departure.Date == date.Date
                     && f.IsActive)
            .AsNoTracking()
            .ToListAsync();

        // Filtra apenas voos com assentos disponíveis
        var availableFlights = flights
            .Where(f =>
            {
                if (f.Airplane?.Armchairs == null)
                    return false;

                var reservedSeats = f.Reservations
                    .Select(r => r.ReservedArmchair)
                    .ToList();

                // Assentos disponíveis = total da aeronave menos os já reservados
                var reservedSeatIds = f.Reservations
                .Where(r => r.ReservedArmchair != null)
                .Select(r => r.ReservedArmchair.Id)
                .ToHashSet();

                var availableSeats = f.Airplane.Armchairs
                    .Where(a => !reservedSeatIds.Contains(a.Id))
                    .ToList();

                return availableSeats.Any(); // só retorna voo se tiver pelo menos um assento livre
            })
            .ToList();

        return availableFlights;
    }

    public async Task<IEnumerable<Flight>> GetDirectFlightsAsync()
    {
        return await _context.Flights
            .Where(f => !f.ExistScale || f.Scales == null || !f.Scales.Any())
            .OrderBy(f => f.Departure)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Flight>> GetByRouteDirectFlightsAsync(string origin, string destination)
    {
        return await _context.Flights
            .Where(f => f.Origin == origin && f.Destination == destination
                   && (!f.ExistScale || f.Scales == null || !f.Scales.Any()))
            .OrderBy(f => f.Departure)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Flight?> GetByIdWithScales(Guid id)
    {
        return await _context.Flights
            .Include(f => f.Airplane)
            .Include(f => f.Scales)
            .Include(f => f.Reservations)
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Flight?> GetByCode(string codeFlight)
    {
        
        return await _context.Flights
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.CodeFlight == codeFlight);
    }
}
