using API.Domain.Entities;

namespace API.Domain.Interface
{
    public interface IConteudoTrilhaUsuarioRepository
    {
        public Task<IEnumerable<ConteudoTrilhaUsuario>> PegarTodasOsConteudosTrilhaUsuario(string IdUsuario, string IdTrilha);
        public Task<ConteudoTrilhaUsuario> PegarConteudoTrilhaUsuario(string IdUsuario, string IdConteudo);

        public Task ConcluirConteudoTrilhaUsuario(string IdUsuario, string IdConteudo);
    }
}
