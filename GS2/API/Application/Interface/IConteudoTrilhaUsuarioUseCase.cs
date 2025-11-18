using API.Domain.Entities;

namespace API.Application.Interface
{
    public interface IConteudoTrilhaUsuarioUseCase
    {
        public Task<IEnumerable<ConteudoTrilhaUsuario>> PegarTodasOsConteudosTrilhaUsuario(string IdUsuario, string IdTrilha);
    }
}
