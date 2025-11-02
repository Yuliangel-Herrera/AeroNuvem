using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Repository;
public class ReservationRepository(AppDBContext context) : IReservationRepository
{
    private readonly AppDBContext _context = context;

    public async Task CreateAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Reservation>?> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Reservations
            .OrderBy(r => r.DateReservation)
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByCode(string CodeRersevation)
    {
        return await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.CodeRersevation == CodeRersevation);
    }

    public async Task<Reservation?> GetByIdAsync(Guid Id)
    {
        var reservation = await _context.Reservations.FindAsync(Id);
        return reservation;
    }

    // Todas as reservas de um determinado voo
    public async Task<IEnumerable<Reservation>> GetReservationsByFlightIdAsync(Guid flightId)
    {
        return await _context.Reservations
            .Include(r => r.Client) 
            .Include(r => r.Flight)
            .Where(r => r.FlightId == flightId)
            .ToListAsync();
    }

}
