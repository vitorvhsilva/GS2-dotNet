using API.Application.Interface;
using API.Domain.Entities;
using API.Domain.Interface;
using Microsoft.Extensions.Logging;

namespace API.Application.Service
{
    public class TrilhaUsuarioUseCase : ITrilhaUsuarioUseCase
    {
        private readonly ITrilhaUsuarioRepository _trilhaUsuarioRepository;
        private readonly ILogger<TrilhaUsuarioUseCase> _logger;

        public TrilhaUsuarioUseCase(
            ITrilhaUsuarioRepository trilhaUsuarioRepository,
            ILogger<TrilhaUsuarioUseCase> logger)
        {
            _trilhaUsuarioRepository = trilhaUsuarioRepository;
            _logger = logger;
        }

        public async Task<TrilhaUsuario> PegarTrilhaDoUsuario(string IdUsuario, string IdTrilha)
        {
            _logger.LogInformation(
                "Buscando trilha {IdTrilha} do usuário {IdUsuario}",
                IdTrilha, IdUsuario);

            var trilhaUsuario = await _trilhaUsuarioRepository
                .PegarTrilhaDoUsuario(IdUsuario, IdTrilha);

            if (trilhaUsuario == null)
            {
                _logger.LogWarning(
                    "Nenhuma trilha {IdTrilha} encontrada para o usuário {IdUsuario}",
                    IdTrilha, IdUsuario);
            }

            return trilhaUsuario;
        }

        public async Task<IEnumerable<TrilhaUsuario>> PegarTrilhasDoUsuario(string IdUsuario)
        {
            _logger.LogInformation(
                "Buscando trilhas para o usuário {IdUsuario}", IdUsuario);

            var trilhas = await _trilhaUsuarioRepository.PegarTrilhasDoUsuario(IdUsuario);

            _logger.LogInformation("Total de trilhas encontradas: {Quantidade}",
                trilhas.Count());

            return trilhas;
        }
    }
}
