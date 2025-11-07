using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Repository;
public class AirplaneRepository(AppDBContext context) : IAirplaneRepository
{
    private readonly AppDBContext _context = context;

    public async Task CreateAsync(Airplane airplane)
    {
        await _context.Airplanes.AddAsync(airplane);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Airplane airplane)
    {
        _context.Airplanes.Update(airplane);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var airplane = await _context.Airplanes.FindAsync(id);
        if (airplane != null)
        {
            _context.Airplanes.Remove(airplane);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Airplane?> GetById(Guid id)
    {
        return await _context.Airplanes
            .Include(a => a.Flights)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Airplane?> GetByPrefix(string prefix)
    {
        return await _context.Airplanes
            .FirstOrDefaultAsync(a => a.Prefix == prefix);
    }

    public async Task<List<Airplane>> GetAll(int pageNumber, int pageSize)
    {
        return await _context.Airplanes
            .Include(a => a.Flights)
            .AsNoTracking()
            .OrderBy(a => a.Prefix) 
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> PrefixExists(string prefix)
    {
        return await _context.Airplanes.AnyAsync(a => a.Prefix == prefix);
    }

    // Consulta de voos programados para uma aeronave específica
    public async Task<List<Flight>> GetFlightScheduleAsync(Guid airplaneId)
    {
        return await _context.Flights
            .Where(f => f.AirplaneId == airplaneId)
            .Include(f => f.Origin)
            .Include(f => f.Destination)
            .OrderBy(f => f.Departure)
            .ToListAsync();
    }
}