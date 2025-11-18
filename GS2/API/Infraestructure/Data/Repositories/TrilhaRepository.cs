using API.Domain.Entities;
using API.Domain.Interface;
using API.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.Repositories
{
    public class TrilhaRepository : ITrilhaRepository
    {
        private readonly ApplicationDbContext _context;
        public TrilhaRepository(ApplicationDbContext context)
        {
            _context = context;
        }   
        public async Task<IEnumerable<Trilha>> PegarTodasAsTrilhas()
        {
            return await _context.Trilhas.ToListAsync();
        }

        public async  Task<Trilha> PegarTrilha(string IdTrilha)
        {
            return await _context.Trilhas.FirstOrDefaultAsync(t => t.IdTrilha == IdTrilha);
        }
    }
}
