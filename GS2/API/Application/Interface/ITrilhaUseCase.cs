using API.Domain.Entities;

namespace API.Application.Interface
{
    public interface ITrilhaUseCase
    {
        public Task<IEnumerable<Trilha>> PegarTodasAsTrilhas();
        public Task<Trilha> PegarTrilha(string IdTrilha);
    }
}
