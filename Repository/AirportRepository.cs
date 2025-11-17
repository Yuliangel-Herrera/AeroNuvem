using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Repository;
public class AirportRepository(AppDBContext context) : IAirportRepository
{
    private readonly AppDBContext _context = context;
    public async Task CreateAsync(Airport airport)
    {
        await _context.Airports.AddAsync(airport);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Airport airport)
    {
        _context.Airports.Update(airport);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var airport = await _context.Airports.FindAsync(id);
        if (airport != null)
        {
            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Airport?> GetByIdAsync(Guid id)
    {
        return await _context.Airports
            .Include(a => a.Adress)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Airport?> GetByIATA(string IATA)
    {
        return await _context.Airports
            .Include(a => a.Adress)
            .FirstOrDefaultAsync(a => a.IATA == IATA);
    }

    public async Task<List<Airport>> GetAll(int pageNumber, int pageSize)
    {
        return await _context.Airports
            .Include(a => a.Adress)
            .AsNoTracking()
            .OrderBy(a => a.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}
