using API.Application.Interface;
using API.Application.Util;
using API.Presentation.Dto.Output;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuarios/")]
    [ApiController]
    public class TrilhaUsuarioController : ControllerBase
    {
        private readonly ILogger<TrilhaUsuarioController> _logger;
        private readonly ITrilhaUsuarioUseCase _trilhaUsuarioUseCase;
        private readonly ITrilhaUseCase _trilhaUseCase;

        public TrilhaUsuarioController(
            ILogger<TrilhaUsuarioController> logger,
            ITrilhaUsuarioUseCase trilhaUsuarioUseCase,
            ITrilhaUseCase trilhaUseCase)
        {
            _logger = logger;
            _trilhaUsuarioUseCase = trilhaUsuarioUseCase;
            _trilhaUseCase = trilhaUseCase;
        }

        [HttpGet("{IdUsuario}/trilhas")]
        public async Task<IActionResult> PegarTrilhasDoUsuario(
            string IdUsuario,
            int Pagina = 1,
            int Tamanho = 5)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando busca de trilhas para o usuário {IdUsuario} (pagina {Pagina}, tamanho {Tamanho})",
                    IdUsuario, Pagina, Tamanho);

                var trilhasUsuario = await _trilhaUsuarioUseCase.PegarTrilhasDoUsuario(IdUsuario);
                var trilhas = await _trilhaUseCase.PegarTodasAsTrilhas();

                var trilhasDict = trilhas.ToDictionary(t => t.IdTrilha, t => t);

                var trilhasCompletas = trilhasUsuario
                    .Where(tu => trilhasDict.ContainsKey(tu.IdTrilha))
                    .Select(tu => new TrilhaUsuarioCompleta(
                        tu.IdTrilha,
                        trilhasDict[tu.IdTrilha].NomeTrilha,
                        trilhasDict[tu.IdTrilha].QuantidadeConteudoTrilha,
                        StringUtil.boolean(tu.TrilhaConcluidaUsuario)
                    ))
                    .ToList();

                var response = PaginacaoResponse<TrilhaUsuarioCompleta>.Criar(
                    trilhasCompletas,
                    Pagina,
                    Tamanho
                );

                _logger.LogInformation(
                    "Busca concluída para o usuário {IdUsuario}. Total de trilhas retornadas: {Total}",
                    IdUsuario, trilhasCompletas.Count);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao buscar trilhas do usuário {IdUsuario}. {Mensagem}",
                    IdUsuario, ex.Message);

                return StatusCode(500, "Erro inesperado ao buscar trilhas do usuário.");
            }
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}")]
        public async Task<IActionResult> PegarTrilhaDoUsuario(
            string IdUsuario,
            string IdTrilha)
        {
            try
            {
                _logger.LogInformation(
                    "Buscando trilha {IdTrilha} para o usuário {IdUsuario}",
                    IdTrilha, IdUsuario);

                var trilhaUsuario =
                    await _trilhaUsuarioUseCase.PegarTrilhaDoUsuario(IdUsuario, IdTrilha);

                var trilha = await _trilhaUseCase.PegarTrilha(IdTrilha);

                if (trilha == null || trilhaUsuario == null)
                {
                    _logger.LogWarning(
                        "Trilha {IdTrilha} não encontrada para o usuário {IdUsuario}",
                        IdTrilha, IdUsuario);

                    return NotFound();
                }

                var dados = new TrilhaUsuarioCompleta(
                    IdTrilha,
                    trilha.NomeTrilha,
                    trilha.QuantidadeConteudoTrilha,
                    StringUtil.boolean(trilhaUsuario.TrilhaConcluidaUsuario)
                );

                var response = new HateoasResponse<TrilhaUsuarioCompleta>(dados);

                response.AddLink(
                    "self",
                    Url.Action(nameof(PegarTrilhaDoUsuario),
                        new { IdUsuario, IdTrilha })!,
                    "GET"
                );

                response.AddLink(
                    "trilhasDoUsuario",
                    Url.Action(nameof(PegarTrilhasDoUsuario),
                        new { IdUsuario })!,
                    "GET"
                );

                response.AddLink(
                    "conteudosDaTrilha",
                    Url.Action("PegarTodasOsConteudosTrilhaUsuario", "ConteudoTrilhaUsuario",
                        new { IdUsuario, IdTrilha })!,
                    "GET"
                );

                if (!StringUtil.boolean(trilhaUsuario.TrilhaConcluidaUsuario))
                {
                    response.AddLink(
                        "concluirTrilha",
                        Url.Action("ConcluirTrilhaUsuario", "TrilhaUsuario",
                            new { IdUsuario, IdTrilha })!,
                        "PATCH"
                    );
                }

                _logger.LogInformation(
                    "Trilha {IdTrilha} retornada com sucesso para o usuário {IdUsuario}",
                    IdTrilha, IdUsuario);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erro ao buscar trilha {IdTrilha} do usuário {IdUsuario}. {Mensagem}",
                    IdTrilha, IdUsuario, ex.Message);

                return StatusCode(500, "Erro inesperado ao buscar trilha do usuário.");
            }
        }
    }
}
