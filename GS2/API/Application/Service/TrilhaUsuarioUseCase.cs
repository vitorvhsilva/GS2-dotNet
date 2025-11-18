using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class TrilhaUsuarioUseCase : ITrilhaUsuarioUseCase
    {
        private readonly ITrilhaUsuarioRepository _trilhaUsuarioRepository;
        public TrilhaUsuarioUseCase(ITrilhaUsuarioRepository trilhaUsuarioRepository)
        {
            _trilhaUsuarioRepository = trilhaUsuarioRepository;
        }

        public async Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario)
        {
            return await _trilhaUsuarioRepository.PegarTrilhasDoUsuario(IdUsuario);
        }
    }
}
