using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class ConteudoTrilhaUseCase : IConteudoTrilhaUseCase
    {
        private readonly IConteudoTrilhaRepository _conteudoTrilhaRepository;
        public ConteudoTrilhaUseCase(IConteudoTrilhaRepository conteudoTrilhaRepository)
        {
            _conteudoTrilhaRepository = conteudoTrilhaRepository;
        }

        public async Task<ConteudoTrilha> PegarConteudoTrilha(string IdConteudo)
        {
            return await _conteudoTrilhaRepository.PegarConteudoTrilha(IdConteudo);
        }

        public async Task<IEnumerable<ConteudoTrilha>> PegarTodasOsConteudosTrilha(string IdTrilha)
        {
            return await _conteudoTrilhaRepository.PegarTodasOsConteudosTrilha(IdTrilha);
        }
    }
}
