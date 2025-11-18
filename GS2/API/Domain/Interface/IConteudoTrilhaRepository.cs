using API.Domain.Entities;

namespace API.Domain.Interface
{
    public interface IConteudoTrilhaRepository
    {
        public Task<IEnumerable<ConteudoTrilha>> PegarTodasOsConteudosTrilha(string IdTrilha);
    }
}
