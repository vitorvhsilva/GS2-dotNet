using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;

namespace API.Application.Service
{
    public class ConteudoTrilhaUseCase : IConteudoTrilhaUseCase
    {
        private readonly ILogger<ConteudoTrilhaUseCase> _logger;
        private readonly IConteudoTrilhaRepository _conteudoTrilhaRepository;

        public ConteudoTrilhaUseCase(
            ILogger<ConteudoTrilhaUseCase> logger,
            IConteudoTrilhaRepository conteudoTrilhaRepository)
        {
            _logger = logger;
            _conteudoTrilhaRepository = conteudoTrilhaRepository;
        }

        public async Task<ConteudoTrilha> PegarConteudoTrilha(string IdConteudo)
        {
            try
            {
                _logger.LogInformation("Obtendo conteúdo da trilha {IdConteudo}", IdConteudo);

                var resultado = await _conteudoTrilhaRepository.PegarConteudoTrilha(IdConteudo);

                if (resultado == null)
                {
                    _logger.LogWarning("Conteúdo da trilha {IdConteudo} não encontrado", IdConteudo);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao obter o conteúdo da trilha {IdConteudo}. Detalhes: {Mensagem}",
                    IdConteudo, ex.Message);

                throw;
            }
        }

        public async Task<IEnumerable<ConteudoTrilha>> PegarTodasOsConteudosTrilha(string IdTrilha)
        {
            try
            {
                _logger.LogInformation("Obtendo todos os conteúdos da trilha {IdTrilha}", IdTrilha);

                return await _conteudoTrilhaRepository.PegarTodasOsConteudosTrilha(IdTrilha);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao obter conteúdos da trilha {IdTrilha}. Detalhes: {Mensagem}",
                    IdTrilha, ex.Message);

                throw;
            }
        }
    }
}
