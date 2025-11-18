using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class ConteudoTrilhaUsuarioUseCase : IConteudoTrilhaUsuarioUseCase
    {
        private readonly IConteudoTrilhaUsuarioRepository _conteudoTrilhaUsuarioRepository;
        private readonly ITrilhaUsuarioRepository _trilhaUsuarioRepository;
        public ConteudoTrilhaUsuarioUseCase(IConteudoTrilhaUsuarioRepository conteudoTrilhaUsuarioRepository, ITrilhaUsuarioRepository trilhaUsuarioRepository)
        {
            _conteudoTrilhaUsuarioRepository = conteudoTrilhaUsuarioRepository;
            _trilhaUsuarioRepository = trilhaUsuarioRepository;
        }

        public async Task ConcluirConteudoTrilhaUsuario(string IdUsuario, string IdTrilha,string IdConteudo)
        {
            await _conteudoTrilhaUsuarioRepository.ConcluirConteudoTrilhaUsuario(IdUsuario, IdConteudo);
            var conteudosTrilha = await PegarTodasOsConteudosTrilhaUsuario(IdUsuario, IdTrilha);
            if (!conteudosTrilha.Any(ct => ct.ConteudoTrilhaConcluidaUsuario == "N"))
            {
                await _trilhaUsuarioRepository.ConcluirTrilhaDoUsuario(IdUsuario, IdTrilha);
            }
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
