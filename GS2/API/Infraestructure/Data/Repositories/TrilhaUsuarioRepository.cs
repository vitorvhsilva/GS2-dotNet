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

        public async Task ConcluirTrilhaDoUsuario(string IdUsuario, string IdTrilha)
        {
            var trilhaUsuario = await _context.TrilhasUsuarios.FirstOrDefaultAsync(tu => tu.IdUsuario == IdUsuario && tu.IdTrilha == IdTrilha);
            trilhaUsuario.TrilhaConcluidaUsuario = "S";
            _context.TrilhasUsuarios.Update(trilhaUsuario);
            await _context.SaveChangesAsync();
        }

        public async Task<TrilhaUsuario> PegarTrilhaDoUsuario(string IdUsuario, string IdTrilha)
        {
            return await _context.TrilhasUsuarios.Where(tu => tu.IdUsuario == IdUsuario && tu.IdTrilha == IdTrilha).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario)
        {
            return await _context.TrilhasUsuarios.Where(tu => tu.IdUsuario == IdUsuario).ToListAsync();
        }
    }
}
