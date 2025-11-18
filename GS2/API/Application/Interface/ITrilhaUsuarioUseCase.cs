using API.Domain.Entities;

namespace API.Application.Interface
{
    public interface ITrilhaUsuarioUseCase
    {
        public Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario);
    }
}
