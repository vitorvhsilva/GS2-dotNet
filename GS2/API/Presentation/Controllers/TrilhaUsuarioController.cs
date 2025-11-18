using API.Application.Interface;
using API.Application.Service;
using API.Application.Util;
using API.Domain.Entities;
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
        private readonly ITrilhaUsuarioUseCase _trilhaUsuarioUseCase;
        private readonly ITrilhaUseCase _trilhaUseCase;

        public TrilhaUsuarioController(ITrilhaUsuarioUseCase trilhaUsuarioUseCase, ITrilhaUseCase trilhaUseCase)
        {
            _trilhaUsuarioUseCase = trilhaUsuarioUseCase;
            _trilhaUseCase = trilhaUseCase;
        }

        [HttpGet("{IdUsuario}/trilhas")]
        public async Task<IActionResult> PegarTrilhasDoUsuario(
            string IdUsuario,
            int Pagina = 1,
            int Tamanho = 5)
        {
            var trilhasUsuario = await _trilhaUsuarioUseCase.PegarTrilhasDoUsuario(IdUsuario);
            var trilhas = await _trilhaUseCase.PegarTodasAsTrilhas();

            var trilhasDict = trilhas.ToDictionary(t => t.IdTrilha, t => t);

            var trilhasCompletas = new List<TrilhaUsuarioCompleta>();

            foreach (var tu in trilhasUsuario)
            {
                if (trilhasDict.TryGetValue(tu.IdTrilha, out var trilha))
                {
                    trilhasCompletas.Add(new TrilhaUsuarioCompleta(
                        tu.IdTrilha,
                        trilha.NomeTrilha,
                        trilha.QuantidadeConteudoTrilha,
                        StringUtil.boolean(tu.TrilhaConcluidaUsuario)
                    ));
                }
            }

            if (Pagina < 1) Pagina = 1;
            if (Tamanho < 1) Tamanho = 5;

            var totalItens = trilhasCompletas.Count;
            var itensPaginados = trilhasCompletas
                .Skip((Pagina - 1) * Tamanho)
                .Take(Tamanho)
                .ToList();

            var response = PaginacaoResponse<TrilhaUsuarioCompleta>.Criar(
                trilhasCompletas,
                Pagina,
                Tamanho
            );

            return Ok(response);
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}")]
        public async Task<IActionResult> PegarTrilhaDoUsuario(
            string IdUsuario,
            string IdTrilha)
        {
            var trilhaUsuario = await _trilhaUsuarioUseCase.PegarTrilhaDoUsuario(IdUsuario, IdTrilha);
            var trilha = await _trilhaUseCase.PegarTrilha(IdTrilha);

            if (trilha == null || trilhaUsuario == null)
            {
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

            return Ok(response);
        }
    }
}
