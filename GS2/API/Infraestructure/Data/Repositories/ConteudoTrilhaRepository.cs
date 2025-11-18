using API.Domain.Entities;
using API.Domain.Interface;
using API.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.Repositories
{
    public class ConteudoTrilhaRepository : IConteudoTrilhaRepository
    {
        private readonly ApplicationDbContext _context;
        public ConteudoTrilhaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConteudoTrilha>> PegarTodasOsConteudosTrilha(string IdTrilha)
        {
            return await _context.ConteudosTrilha.Where(ct => ct.IdTrilha == IdTrilha).ToListAsync();
        }
    }
}
