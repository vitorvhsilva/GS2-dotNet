using API.Domain.Entities;

namespace API.Domain.Interface
{
    public interface ITrilhaRepository
    {
        public Task<IEnumerable<Trilha>> PegarTodasAsTrilhas();
    }
}
