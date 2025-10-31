using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Repository
{
    public class AirplaneRepository : IAirplaneRepository
    {
        private readonly AppDBContext _context;

        public AirplaneRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task Create(Airplane airplane)
        {
            await _context.Airplanes.AddAsync(airplane);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Airplane airplane)
        {
            _context.Airplanes.Update(airplane);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane != null)
            {
                _context.Airplanes.Remove(airplane);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Airplane> GetById(Guid id)
        {
            return await _context.Airplanes
                .Include(a => a.Flights)
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new InvalidOperationException($"Aeronave com Id {id} não encontrada.");
        }

        public async Task<Airplane?> GetByPrefix(string prefix)
        {
            return await _context.Airplanes
                .FirstOrDefaultAsync(a => a.Prefix == prefix);
        }

        public async Task<List<Airplane>> GetAll()
        {
            return await _context.Airplanes
                .Include(a => a.Flights)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> PrefixExists(string prefix)
        {
            return await _context.Airplanes.AnyAsync(a => a.Prefix == prefix);
        }
    }
}