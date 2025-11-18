using API.Domain.Entities;

namespace API.Application.Interface
{
    public interface IConteudoTrilhaUseCase
    {
        public Task<IEnumerable<ConteudoTrilha>> PegarTodasOsConteudosTrilha(string IdTrilha);
    }
}
