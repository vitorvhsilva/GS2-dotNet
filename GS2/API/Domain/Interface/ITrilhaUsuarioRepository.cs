using API.Domain.Entities;

namespace API.Domain.Interface
{
    public interface ITrilhaUsuarioRepository
    {
        public Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario);
        public Task<TrilhaUsuario> PegarTrilhaDoUsuario(string IdUsuario, string IdTrilha);
        public Task ConcluirTrilhaDoUsuario(string IdUsuario, string IdTrilha);
    }
}
