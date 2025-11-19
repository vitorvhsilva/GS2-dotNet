using API.Application.Interface;
using API.Application.Util;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class ConteudoTrilhaUsuarioUseCase : IConteudoTrilhaUsuarioUseCase
    {
        private readonly ILogger<ConteudoTrilhaUsuarioUseCase> _logger;
        private readonly IConteudoTrilhaUsuarioRepository _conteudoTrilhaUsuarioRepository;
        private readonly ITrilhaUsuarioRepository _trilhaUsuarioRepository;

        public ConteudoTrilhaUsuarioUseCase(
            ILogger<ConteudoTrilhaUsuarioUseCase> logger,
            IConteudoTrilhaUsuarioRepository conteudoTrilhaUsuarioRepository,
            ITrilhaUsuarioRepository trilhaUsuarioRepository)
        {
            _logger = logger;
            _conteudoTrilhaUsuarioRepository = conteudoTrilhaUsuarioRepository;
            _trilhaUsuarioRepository = trilhaUsuarioRepository;
        }

        public async Task ConcluirConteudoTrilhaUsuario(string IdUsuario, string IdTrilha, string IdConteudo)
        {
            try
            {
                _logger.LogInformation(
                    "Usuário {IdUsuario} concluindo conteúdo {IdConteudo} da trilha {IdTrilha}",
                    IdUsuario, IdConteudo, IdTrilha);

                await _conteudoTrilhaUsuarioRepository.ConcluirConteudoTrilhaUsuario(IdUsuario, IdConteudo);

                var conteudos = await PegarTodasOsConteudosTrilhaUsuario(IdUsuario, IdTrilha);

                bool trilhaCompleta = !conteudos.Any(c =>
                    !StringUtil.boolean(c.ConteudoTrilhaConcluidaUsuario));

                if (trilhaCompleta)
                {
                    _logger.LogInformation(
                        "Todos os conteúdos concluídos. Concluindo trilha {IdTrilha} para o usuário {IdUsuario}",
                        IdTrilha, IdUsuario);

                    await _trilhaUsuarioRepository.ConcluirTrilhaDoUsuario(IdUsuario, IdTrilha);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao concluir conteúdo {IdConteudo} da trilha {IdTrilha} para o usuário {IdUsuario}. Detalhes: {Mensagem}",
                    IdConteudo, IdTrilha, IdUsuario, ex.Message);

                throw;
            }
        }

        public async Task<ConteudoTrilhaUsuario> PegarConteudoTrilhaUsuario(string IdUsuario, string IdConteudo)
        {
            try
            {
                _logger.LogInformation(
                    "Buscando conteúdo {IdConteudo} do usuário {IdUsuario}",
                    IdConteudo, IdUsuario);

                return await _conteudoTrilhaUsuarioRepository.PegarConteudoTrilhaUsuario(IdUsuario, IdConteudo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao obter conteúdo {IdConteudo} do usuário {IdUsuario}. {Mensagem}",
                    IdConteudo, IdUsuario, ex.Message);

                throw;
            }
        }

        public async Task<IEnumerable<ConteudoTrilhaUsuario>> PegarTodasOsConteudosTrilhaUsuario(string IdUsuario, string IdTrilha)
        {
            try
            {
                _logger.LogInformation(
                    "Buscando todos os conteúdos da trilha {IdTrilha} para o usuário {IdUsuario}",
                    IdTrilha, IdUsuario);

                return await _conteudoTrilhaUsuarioRepository.PegarTodasOsConteudosTrilhaUsuario(IdUsuario, IdTrilha);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao listar conteúdos da trilha {IdTrilha} para o usuário {IdUsuario}. {Mensagem}",
                    IdTrilha, IdUsuario, ex.Message);

                throw;
            }
        }
    }
}
