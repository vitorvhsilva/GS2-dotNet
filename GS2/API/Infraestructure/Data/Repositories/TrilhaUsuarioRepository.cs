using API.Domain.Entities;
using API.Domain.Interface;
using API.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.Repositories
{
    public class TrilhaUsuarioRepository : ITrilhaUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public TrilhaUsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario)
        {
            return await _context.TrilhasUsuarios.Where(tu => tu.IdUsuario == IdUsuario).ToListAsync();
        }
    }
}
