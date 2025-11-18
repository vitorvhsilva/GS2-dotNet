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

        public ConteudoTrilhaUsuarioController(IConteudoTrilhaUsuarioUseCase conteudoTrilhaUsuarioUseCase, IConteudoTrilhaUseCase conteudoTrilhaUseCase)
        {
            _conteudoTrilhaUsuarioUseCase = conteudoTrilhaUsuarioUseCase;
            _conteudoTrilhaUseCase = conteudoTrilhaUseCase;
        }

        [HttpGet("{IdUsuario}/trilhas/{IdTrilha}")]
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

            var response = new
            {
                data = conteudoTrilhasCompletas
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

            var response = new
            {
                data = new ConteudoTrilhaCompleta(
                    conteudoTrilha.IdConteudoTrilha,
                    conteudoTrilha.NomeConteudoTrilha,
                    conteudoTrilha.TipoConteudoTrilha,
                    conteudoTrilha.TextoConteudoTrilha,
                    StringUtil.boolean(conteudoTrilhaUsuario.ConteudoTrilhaConcluidaUsuario)
                )
            };

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
