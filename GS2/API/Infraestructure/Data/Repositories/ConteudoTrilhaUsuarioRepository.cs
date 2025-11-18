using API.Domain.Entities;
using API.Domain.Interface;
using API.Infraestructure.Data.AppData;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.Repositories
{
    public class ConteudoTrilhaUsuarioRepository : IConteudoTrilhaUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public ConteudoTrilhaUsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ConcluirConteudoTrilhaUsuario(string IdUsuario, string IdConteudo)
        {
            var conteudoTrilha = await _context.ConteudosTrilhaUsuario.FirstOrDefaultAsync(ct => ct.IdUsuario == IdUsuario && ct.IdConteudoTrilha == IdConteudo);
            conteudoTrilha.ConteudoTrilhaConcluidaUsuario = "S";
            _context.Update(conteudoTrilha);
            await _context.SaveChangesAsync();
        }

        public async Task<ConteudoTrilhaUsuario> PegarConteudoTrilhaUsuario(string IdUsuario, string IdConteudo)
        {
            return await _context.ConteudosTrilhaUsuario.FirstOrDefaultAsync(ct => ct.IdUsuario == IdUsuario && ct.IdConteudoTrilha == IdConteudo);
        }

        public async Task<IEnumerable<ConteudoTrilhaUsuario>> PegarTodasOsConteudosTrilhaUsuario(string IdUsuario, string IdTrilha)
        {
            return await _context.ConteudosTrilhaUsuario.Where(ct => ct.IdUsuario == IdUsuario && ct.ConteudoTrilha.IdTrilha == IdTrilha).ToListAsync();
        }
    }
}
