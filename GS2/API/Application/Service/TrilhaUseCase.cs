using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class TrilhaUseCase : ITrilhaUseCase
    {
        private readonly ITrilhaRepository _trilhaUsuarioRepository;
        public TrilhaUseCase(ITrilhaRepository trilhaUsuarioRepository)
        {
            _trilhaUsuarioRepository = trilhaUsuarioRepository;
        }

        public async Task<IEnumerable<Trilha>> PegarTodasAsTrilhas()
        {
            return await _trilhaUsuarioRepository.PegarTodasAsTrilhas();
        }
    }
}
