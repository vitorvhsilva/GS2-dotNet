using API.Domain.Entities;

namespace API.Domain.Interface
{
    public interface ITrilhaUsuarioRepository
    {
        public Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario);
    }
}
