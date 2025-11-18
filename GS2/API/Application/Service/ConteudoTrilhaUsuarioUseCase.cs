using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class ConteudoTrilhaUsuarioUseCase : IConteudoTrilhaUsuarioUseCase
    {
        private readonly IConteudoTrilhaUsuarioRepository _conteudoTrilhaUsuarioRepository;
        public ConteudoTrilhaUsuarioUseCase(IConteudoTrilhaUsuarioRepository conteudoTrilhaUsuarioRepository)
        {
            _conteudoTrilhaUsuarioRepository = conteudoTrilhaUsuarioRepository;
        }

        public async Task<ConteudoTrilhaUsuario> PegarConteudoTrilhaUsuario(string IdUsuario, string IdConteudo)
        {
            return await _conteudoTrilhaUsuarioRepository.PegarConteudoTrilhaUsuario(IdUsuario, IdConteudo);
        }

        public async Task<IEnumerable<ConteudoTrilhaUsuario>> PegarTodasOsConteudosTrilhaUsuario(string IdUsuario, string IdTrilha)
        {
            return await _conteudoTrilhaUsuarioRepository.PegarTodasOsConteudosTrilhaUsuario(IdUsuario, IdTrilha);
        }
    }
}
