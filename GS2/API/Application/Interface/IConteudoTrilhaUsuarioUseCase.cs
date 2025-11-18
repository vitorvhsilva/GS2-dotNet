using API.Domain.Entities;

namespace API.Application.Interface
{
    public interface IConteudoTrilhaUsuarioUseCase
    {
        public Task<IEnumerable<ConteudoTrilhaUsuario>> PegarTodasOsConteudosTrilhaUsuario(string IdUsuario, string IdTrilha);
        public Task<ConteudoTrilhaUsuario> PegarConteudoTrilhaUsuario(string IdUsuario, string IdConteudo);

        public Task ConcluirConteudoTrilhaUsuario(string IdUsuario, string IdTrilha, string IdConteudo);
    }
}
