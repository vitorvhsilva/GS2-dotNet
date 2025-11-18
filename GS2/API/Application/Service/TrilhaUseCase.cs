using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class TrilhaUseCase : ITrilhaUseCase
    {
        private readonly ITrilhaRepository _trilhaRepository;
        public TrilhaUseCase(ITrilhaRepository trilhaUsuarioRepository)
        {
            _trilhaRepository = trilhaUsuarioRepository;
        }

        public async Task<IEnumerable<Trilha>> PegarTodasAsTrilhas()
        {
            return await _trilhaRepository.PegarTodasAsTrilhas();
        }

        public async Task<Trilha> PegarTrilha(string IdTrilha)
        {
            return await _trilhaRepository.PegarTrilha(IdTrilha);
        }
    }
}
