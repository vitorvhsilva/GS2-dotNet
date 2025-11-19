using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;
using Microsoft.Extensions.Logging;

namespace API.Application.Service
{
    public class TrilhaUseCase : ITrilhaUseCase
    {
        private readonly ITrilhaRepository _trilhaRepository;
        private readonly ILogger<TrilhaUseCase> _logger;

        public TrilhaUseCase(
            ITrilhaRepository trilhaRepository,
            ILogger<TrilhaUseCase> logger)
        {
            _trilhaRepository = trilhaRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Trilha>> PegarTodasAsTrilhas()
        {
            _logger.LogInformation("Buscando todas as trilhas...");

            var trilhas = await _trilhaRepository.PegarTodasAsTrilhas();

            _logger.LogInformation("Total de trilhas encontradas: {Quantidade}",
                trilhas.Count());

            return trilhas;
        }

        public async Task<Trilha> PegarTrilha(string IdTrilha)
        {
            _logger.LogInformation("Buscando trilha com ID: {IdTrilha}", IdTrilha);

            var trilha = await _trilhaRepository.PegarTrilha(IdTrilha);

            if (trilha == null)
            {
                _logger.LogWarning("Nenhuma trilha encontrada com ID: {IdTrilha}", IdTrilha);
            }

            return trilha;
        }
    }
}
