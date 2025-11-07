using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Repository
{
    public class ScaleRepository : IScaleRepository
    {
        private readonly AppDBContext _context;

        public ScaleRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task Create(Scale scale)
        {
            await _context.Scale.AddAsync(scale);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Scale scale)
        {
            _context.Scale.Update(scale);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var scale = await _context.Scale.FindAsync(id);
            if (scale != null)
            {
                _context.Scale.Remove(scale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByFlightId(Guid flightId)
        {
            var scales = await _context.Scale.Where(s => s.FlightId == flightId).ToListAsync();
            _context.Scale.RemoveRange(scales);
            await _context.SaveChangesAsync();
        }

        public async Task<Scale> GetById(Guid id)
        {
            return await _context.Scale
                .Include(s => s.Flight)
                .FirstOrDefaultAsync(s => s.Id == id)
                ?? throw new InvalidOperationException($"Escala com Id {id} não encontrada.");
        }

        public async Task<List<Scale>> GetByFlightId(Guid flightId)
        {
            return await _context.Scale
                .Where(s => s.FlightId == flightId)
                .OrderBy(s => s.Arrival)
                .ToListAsync();
        }

        public async Task<List<Scale>> GetAll()
        {
            return await _context.Scale
                .Include(s => s.Flight)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}