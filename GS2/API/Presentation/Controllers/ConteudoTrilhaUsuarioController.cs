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
    public class ConteudoTrilhaUsuarioController : ControllerBase
    {
        private readonly ILogger<ConteudoTrilhaUsuarioController> _logger;
        private readonly IConteudoTrilhaUsuarioUseCase _conteudoTrilhaUsuarioUseCase;
        private readonly IConteudoTrilhaUseCase _conteudoTrilhaUseCase;
        private readonly ITrilhaUsuarioUseCase _trilhaUsuarioUseCase;

        public ConteudoTrilhaUsuarioController(
            ILogger<ConteudoTrilhaUsuarioController> logger,
            IConteudoTrilhaUsuarioUseCase conteudoTrilhaUsuarioUseCase,
            IConteudoTrilhaUseCase conteudoTrilhaUseCase,
            ITrilhaUsuarioUseCase trilhaUsuarioUseCase)
        {
            _logger = logger;
            _conteudoTrilhaUsuarioUseCase = conteudoTrilhaUsuarioUseCase;
            _conteudoTrilhaUseCase = conteudoTrilhaUseCase;
            _trilhaUsuarioUseCase = trilhaUsuarioUseCase;
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}/conteudos")]
        public async Task<IActionResult> PegarTodasOsConteudosTrilhaUsuario(
            string IdUsuario,
            string IdTrilha)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando busca dos conteúdos da trilha {TrilhaId} do usuário {UsuarioId}",
                    IdTrilha, IdUsuario
                );

                var conteudosTrilhaUsuario = await _conteudoTrilhaUsuarioUseCase
                    .PegarTodasOsConteudosTrilhaUsuario(IdUsuario, IdTrilha);

                var conteudosTrilha = await _conteudoTrilhaUseCase
                    .PegarTodasOsConteudosTrilha(IdTrilha);

                var conteudosTrilhasDict = conteudosTrilha
                    .ToDictionary(ct => ct.IdConteudoTrilha, ct => ct);

                var conteudoTrilhasCompletas = new List<ConteudoTrilhaUsuarioCompleta>();

                foreach (var ctu in conteudosTrilhaUsuario)
                {
                    if (conteudosTrilhasDict.TryGetValue(ctu.IdConteudoTrilha, out var conteudoTrilha))
                    {
                        conteudoTrilhasCompletas.Add(new ConteudoTrilhaUsuarioCompleta(
                            ctu.IdConteudoTrilha,
                            conteudoTrilha.NomeConteudoTrilha,
                            conteudoTrilha.TipoConteudoTrilha,
                            StringUtil.boolean(ctu.ConteudoTrilhaConcluidaUsuario)
                        ));
                    }
                }

                var trilha = await _trilhaUsuarioUseCase
                    .PegarTrilhaDoUsuario(IdUsuario, IdTrilha);

                _logger.LogInformation(
                    "Conteúdos da trilha {TrilhaId} para o usuário {UsuarioId} obtidos com sucesso",
                    IdTrilha, IdUsuario
                );

                return Ok(new
                {
                    data = new
                    {
                        trilhaConcluida = StringUtil.boolean(trilha.TrilhaConcluidaUsuario),
                        conteudos = conteudoTrilhasCompletas
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao obter conteúdos da trilha {TrilhaId} para o usuário {UsuarioId}",
                    IdTrilha, IdUsuario
                );

                return BadRequest();
            }
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}/conteudos/{IdConteudo}")]
        public async Task<IActionResult> PegarConteudoTrilhaUsuario(
            string IdUsuario,
            string IdTrilha,
            string IdConteudo
        )
        {
            try
            {
                _logger.LogInformation(
                    "Buscando conteúdo {ConteudoId} da trilha {TrilhaId} para o usuário {UsuarioId}",
                    IdConteudo, IdTrilha, IdUsuario
                );

                var conteudoTrilhaUsuario = await _conteudoTrilhaUsuarioUseCase
                    .PegarConteudoTrilhaUsuario(IdUsuario, IdConteudo);

                var conteudoTrilha = await _conteudoTrilhaUseCase
                    .PegarConteudoTrilha(IdConteudo);

                if (conteudoTrilha == null || conteudoTrilhaUsuario == null)
                {
                    _logger.LogWarning(
                        "Conteúdo {ConteudoId} não encontrado para o usuário {UsuarioId} na trilha {TrilhaId}",
                        IdConteudo, IdUsuario, IdTrilha
                    );

                    return NotFound();
                }

                var conteudosTrilha = await _conteudoTrilhaUseCase
                    .PegarTodasOsConteudosTrilha(IdTrilha);

                var listaOrdenada = conteudosTrilha.OrderBy(c => c.IdConteudoTrilha).ToList();
                int indexAtual = listaOrdenada.FindIndex(c => c.IdConteudoTrilha == IdConteudo);

                string linkNext =
                    indexAtual >= 0 && indexAtual < listaOrdenada.Count - 1
                        ? Url.Action(nameof(PegarConteudoTrilhaUsuario),
                            new { IdUsuario, IdTrilha, IdConteudo = listaOrdenada[indexAtual + 1].IdConteudoTrilha })
                        : null;

                string linkPrev =
                    indexAtual > 0
                        ? Url.Action(nameof(PegarConteudoTrilhaUsuario),
                            new { IdUsuario, IdTrilha, IdConteudo = listaOrdenada[indexAtual - 1].IdConteudoTrilha })
                        : null;

                var data = new ConteudoTrilhaCompleta(
                    conteudoTrilha.IdConteudoTrilha,
                    conteudoTrilha.NomeConteudoTrilha,
                    conteudoTrilha.TipoConteudoTrilha,
                    conteudoTrilha.TextoConteudoTrilha,
                    StringUtil.boolean(conteudoTrilhaUsuario.ConteudoTrilhaConcluidaUsuario)
                );

                var response = new HateoasResponse<ConteudoTrilhaCompleta>(data);

                response.AddLink("self",
                    Url.Action(nameof(PegarConteudoTrilhaUsuario),
                        new { IdUsuario, IdTrilha, IdConteudo }),
                    "GET");

                response.AddLink("listarConteudos",
                    Url.Action("PegarTodasOsConteudosTrilhaUsuario", "ConteudoTrilhaUsuario",
                        new { IdUsuario, IdTrilha }),
                    "GET");

                if (!StringUtil.boolean(conteudoTrilhaUsuario.ConteudoTrilhaConcluidaUsuario))
                {
                    response.AddLink("concluirConteudo",
                        Url.Action(nameof(ConcluirConteudoTrilhaUsuario),
                            new { IdUsuario, IdTrilha, IdConteudo }),
                        "PATCH");
                }

                response.AddLink("proximo", linkNext, "GET");
                response.AddLink("anterior", linkPrev, "GET");

                _logger.LogInformation(
                    "Conteúdo {ConteudoId} da trilha {TrilhaId} retornado com sucesso para o usuário {UsuarioId}",
                    IdConteudo, IdTrilha, IdUsuario
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao obter conteúdo {ConteudoId} da trilha {TrilhaId} para o usuário {UsuarioId}",
                    IdConteudo, IdTrilha, IdUsuario
                );

                return BadRequest();
            }
        }

        [HttpPatch("{IdUsuario}/trilhas/{IdTrilha}/conteudos/{IdConteudo}")]
        public async Task<IActionResult> ConcluirConteudoTrilhaUsuario(
            string IdUsuario,
            string IdTrilha,
            string IdConteudo
        )
        {
            try
            {
                _logger.LogInformation(
                    "Tentando concluir conteúdo {ConteudoId} da trilha {TrilhaId} para o usuário {UsuarioId}",
                    IdConteudo, IdTrilha, IdUsuario
                );

                await _conteudoTrilhaUsuarioUseCase
                    .ConcluirConteudoTrilhaUsuario(IdUsuario, IdTrilha, IdConteudo);

                _logger.LogInformation(
                    "Conteúdo {ConteudoId} concluído com sucesso pelo usuário {UsuarioId}",
                    IdConteudo, IdUsuario
                );

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao concluir conteúdo {ConteudoId} da trilha {TrilhaId} para o usuário {UsuarioId}",
                    IdConteudo, IdTrilha, IdUsuario
                );

                return BadRequest();
            }
        }
    }
}
