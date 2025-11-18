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
        private readonly IConteudoTrilhaUsuarioUseCase _conteudoTrilhaUsuarioUseCase;
        private readonly IConteudoTrilhaUseCase _conteudoTrilhaUseCase;
        private readonly ITrilhaUsuarioUseCase _trilhaUsuarioUseCase;

        public ConteudoTrilhaUsuarioController(IConteudoTrilhaUsuarioUseCase conteudoTrilhaUsuarioUseCase, IConteudoTrilhaUseCase conteudoTrilhaUseCase, ITrilhaUsuarioUseCase trilhaUsuarioUseCase)
        {
            _conteudoTrilhaUsuarioUseCase = conteudoTrilhaUsuarioUseCase;
            _conteudoTrilhaUseCase = conteudoTrilhaUseCase;
            _trilhaUsuarioUseCase = trilhaUsuarioUseCase;
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}/conteudos")]
        public async Task<IActionResult> PegarTodasOsConteudosTrilhaUsuario(
            string IdUsuario,
            string IdTrilha)
        {
            var conteudosTrilhaUsuario = await _conteudoTrilhaUsuarioUseCase.PegarTodasOsConteudosTrilhaUsuario(IdUsuario, IdTrilha);
            var conteudosTrilha = await _conteudoTrilhaUseCase.PegarTodasOsConteudosTrilha(IdTrilha);

            var conteudosTrilhasDict = conteudosTrilha.ToDictionary(ct => ct.IdConteudoTrilha, ct => ct);

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

            var trilha = await _trilhaUsuarioUseCase.PegarTrilhaDoUsuario(IdUsuario, IdTrilha);

            var response = new
            {
                data = new {
                    trilhaConcluida = StringUtil.boolean(trilha.TrilhaConcluidaUsuario),
                    conteudos = conteudoTrilhasCompletas
                }
            };

            return Ok(response);
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}/conteudos/{IdConteudo}")]
        public async Task<IActionResult> PegarConteudoTrilhaUsuario(
            string IdUsuario,
            string IdTrilha,
            string IdConteudo
        )
        {
            var conteudoTrilhaUsuario = await _conteudoTrilhaUsuarioUseCase.PegarConteudoTrilhaUsuario(IdUsuario, IdConteudo);
            var conteudoTrilha = await _conteudoTrilhaUseCase.PegarConteudoTrilha(IdConteudo);

            if (conteudoTrilha == null || conteudoTrilhaUsuario == null)
            {
                return NotFound();
            }

            var conteudosTrilha = await _conteudoTrilhaUseCase.PegarTodasOsConteudosTrilha(IdTrilha);
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
                Url.Action(nameof(PegarConteudoTrilhaUsuario), new { IdUsuario, IdTrilha, IdConteudo }),
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

            return Ok(response);
        }

        [HttpPatch("{IdUsuario}/trilhas/{IdTrilha}/conteudos/{IdConteudo}")]
        public async Task<IActionResult> ConcluirConteudoTrilhaUsuario(
            string IdUsuario,
            string IdTrilha,
            string IdConteudo
            )
        {
            await _conteudoTrilhaUsuarioUseCase.ConcluirConteudoTrilhaUsuario(IdUsuario, IdTrilha, IdConteudo);

            return Ok();
        }
    }
}
