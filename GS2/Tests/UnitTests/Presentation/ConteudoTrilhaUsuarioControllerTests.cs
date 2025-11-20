using API.Application.Interface;
using API.Application.Util;
using API.Domain.Entities;
using API.Presentation.Controllers;
using API.Presentation.Dto.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Tests.UnitTest;
using Xunit;

namespace Tests.UnitTests.Presentation
{
    public class ConteudoTrilhaUsuarioControllerTests
    {
        private readonly Mock<IConteudoTrilhaUsuarioUseCase> _conteudoTrilhaUsuarioUseCaseMock;
        private readonly Mock<IConteudoTrilhaUseCase> _conteudoTrilhaUseCaseMock;
        private readonly Mock<ITrilhaUsuarioUseCase> _trilhaUsuarioUseCaseMock;
        private readonly ConteudoTrilhaUsuarioController _controller;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        public ConteudoTrilhaUsuarioControllerTests()
        {
            _conteudoTrilhaUsuarioUseCaseMock = new Mock<IConteudoTrilhaUsuarioUseCase>();
            _conteudoTrilhaUseCaseMock = new Mock<IConteudoTrilhaUseCase>();
            _trilhaUsuarioUseCaseMock = new Mock<ITrilhaUsuarioUseCase>();
            _urlHelperMock = new Mock<IUrlHelper>();
            
            _controller = new ConteudoTrilhaUsuarioController(
                new MockLogger<ConteudoTrilhaUsuarioController>(),
                _conteudoTrilhaUsuarioUseCaseMock.Object,
                _conteudoTrilhaUseCaseMock.Object,
                _trilhaUsuarioUseCaseMock.Object
            );

            // Mock Url.Action para retornar URLs válidas
            _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                         .Returns("/api/v1/usuarios/usuario-1/trilhas/trilha-1/conteudos");

            _controller.Url = _urlHelperMock.Object;
        }

        [Fact(DisplayName = "Deve retornar OK com lista de conteúdos da trilha do usuário")]
        public async Task PegarTodasOsConteudosTrilhaUsuario_DeveRetornarOk()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";

            var conteudosUsuario = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = idUsuario, IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S" },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-2", IdUsuario = idUsuario, IdConteudoTrilha = "c-2", ConteudoTrilhaConcluidaUsuario = "N" }
            };

            var conteudos = new List<ConteudoTrilha>
            {
                new ConteudoTrilha { IdConteudoTrilha = "c-1", NomeConteudoTrilha = "Conteúdo 1", TipoConteudoTrilha = "Vídeo", TextoConteudoTrilha = "Texto", IdTrilha = idTrilha },
                new ConteudoTrilha { IdConteudoTrilha = "c-2", NomeConteudoTrilha = "Conteúdo 2", TipoConteudoTrilha = "Artigo", TextoConteudoTrilha = "Texto", IdTrilha = idTrilha }
            };

            var trilhaUsuario = new TrilhaUsuario 
            { 
                IdTrilhaUsuario = "tu-1", 
                IdUsuario = idUsuario, 
                IdTrilha = idTrilha, 
                TrilhaConcluidaUsuario = "N" 
            };

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha))
                                             .ReturnsAsync(conteudosUsuario);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarTodasOsConteudosTrilha(idTrilha))
                                     .ReturnsAsync(conteudos);

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhaDoUsuario(idUsuario, idTrilha))
                                    .ReturnsAsync(trilhaUsuario);

            // Act
            var result = await _controller.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult.Value);
        }

        [Fact(DisplayName = "Deve retornar OK com conteúdo específico da trilha do usuário")]
        public async Task PegarConteudoTrilhaUsuario_DeveRetornarOkQuandoExistir()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";
            var idConteudo = "conteudo-1";

            var conteudoUsuario = new ConteudoTrilhaUsuario 
            { 
                IdConteudoTrilhaUsuario = "ctu-1", 
                IdUsuario = idUsuario, 
                IdConteudoTrilha = idConteudo, 
                ConteudoTrilhaConcluidaUsuario = "N" 
            };

            var conteudo = new ConteudoTrilha 
            { 
                IdConteudoTrilha = idConteudo, 
                NomeConteudoTrilha = "Conteúdo Teste", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Texto do conteúdo",
                IdTrilha = idTrilha 
            };

            var conteudosTrilha = new List<ConteudoTrilha> { conteudo };

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.PegarConteudoTrilhaUsuario(idUsuario, idConteudo))
                                             .ReturnsAsync(conteudoUsuario);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarConteudoTrilha(idConteudo))
                                     .ReturnsAsync(conteudo);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarTodasOsConteudosTrilha(idTrilha))
                                     .ReturnsAsync(conteudosTrilha);

            // Act
            var result = await _controller.PegarConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var response = Assert.IsType<HateoasResponse<ConteudoTrilhaCompleta>>(okResult.Value);
            Assert.NotNull(response.Data);
        }

        [Fact(DisplayName = "Deve retornar NotFound quando conteúdo não existir")]
        public async Task PegarConteudoTrilhaUsuario_DeveRetornarNotFoundQuandoNaoExistir()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";
            var idConteudo = "conteudo-inexistente";

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.PegarConteudoTrilhaUsuario(idUsuario, idConteudo))
                                             .ReturnsAsync((ConteudoTrilhaUsuario)null!);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarConteudoTrilha(idConteudo))
                                     .ReturnsAsync((ConteudoTrilha)null!);

            // Act
            var result = await _controller.PegarConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Deve retornar Ok ao concluir conteúdo")]
        public async Task ConcluirConteudoTrilhaUsuario_DeveRetornarOk()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";
            var idConteudo = "conteudo-1";

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo))
                                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact(DisplayName = "Deve retornar BadRequest ao concluir conteúdo com erro")]
        public async Task ConcluirConteudoTrilhaUsuario_DeveRetornarBadRequestQuandoErro()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";
            var idConteudo = "conteudo-1";

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo))
                                             .ThrowsAsync(new Exception("Erro ao concluir"));

            // Act
            var result = await _controller.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "Deve incluir links HATEOAS com navegação")]
        public async Task PegarConteudoTrilhaUsuario_DeveIncluirLinksHateoasComNavegacao()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";
            var idConteudo = "c-1";

            var conteudoUsuario = new ConteudoTrilhaUsuario 
            { 
                IdConteudoTrilhaUsuario = "ctu-1", 
                IdUsuario = idUsuario, 
                IdConteudoTrilha = idConteudo, 
                ConteudoTrilhaConcluidaUsuario = "N" 
            };

            var conteudo = new ConteudoTrilha 
            { 
                IdConteudoTrilha = idConteudo, 
                NomeConteudoTrilha = "Conteúdo 1", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = idTrilha 
            };

            var conteudo2 = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "c-2", 
                NomeConteudoTrilha = "Conteúdo 2", 
                TipoConteudoTrilha = "Artigo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = idTrilha 
            };

            var conteudosTrilha = new List<ConteudoTrilha> { conteudo, conteudo2 };

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.PegarConteudoTrilhaUsuario(idUsuario, idConteudo))
                                             .ReturnsAsync(conteudoUsuario);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarConteudoTrilha(idConteudo))
                                     .ReturnsAsync(conteudo);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarTodasOsConteudosTrilha(idTrilha))
                                     .ReturnsAsync(conteudosTrilha);

            // Act
            var result = await _controller.PegarConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var response = Assert.IsType<HateoasResponse<ConteudoTrilhaCompleta>>(okResult.Value);
            Assert.NotNull(response.Links);
            // Validar que tem links (URL mockado retorna string válida)
            Assert.True(response.Links.Count >= 4, "Deve ter ao menos 4 links (self, listarConteudos, proximo, anterior)");
        }

        [Fact(DisplayName = "Deve retornar BadRequest ao buscar conteúdos com erro")]
        public async Task PegarTodasOsConteudosTrilhaUsuario_DeveRetornarBadRequestQuandoErro()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha))
                                             .ThrowsAsync(new Exception("Erro no repositório"));

            // Act
            var result = await _controller.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact(DisplayName = "Deve incluir link concluirConteudo apenas se não concluído")]
        public async Task PegarConteudoTrilhaUsuario_DeveIncluirLinkConcluirApenasSeNaoConcluido()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";
            var idConteudo = "conteudo-1";

            var conteudoUsuario = new ConteudoTrilhaUsuario 
            { 
                IdConteudoTrilhaUsuario = "ctu-1", 
                IdUsuario = idUsuario, 
                IdConteudoTrilha = idConteudo, 
                ConteudoTrilhaConcluidaUsuario = "S" // Já concluído
            };

            var conteudo = new ConteudoTrilha 
            { 
                IdConteudoTrilha = idConteudo, 
                NomeConteudoTrilha = "Conteúdo Teste", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = idTrilha 
            };

            var conteudosTrilha = new List<ConteudoTrilha> { conteudo };

            _conteudoTrilhaUsuarioUseCaseMock.Setup(x => x.PegarConteudoTrilhaUsuario(idUsuario, idConteudo))
                                             .ReturnsAsync(conteudoUsuario);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarConteudoTrilha(idConteudo))
                                     .ReturnsAsync(conteudo);

            _conteudoTrilhaUseCaseMock.Setup(x => x.PegarTodasOsConteudosTrilha(idTrilha))
                                     .ReturnsAsync(conteudosTrilha);

            // Act
            var result = await _controller.PegarConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var response = Assert.IsType<HateoasResponse<ConteudoTrilhaCompleta>>(okResult.Value);
            
            // Verificar se tem link "concluirConteudo" - não deve ter pois já está concluído
            var hasConcluirLink = response.Links.ContainsKey("concluirConteudo");
            Assert.False(hasConcluirLink, "Não deve incluir link 'concluirConteudo' se já concluído");
        }
    }
}
