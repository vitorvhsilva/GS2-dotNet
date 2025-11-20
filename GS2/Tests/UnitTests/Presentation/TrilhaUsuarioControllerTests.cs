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
    public class TrilhaUsuarioControllerTests
    {
        private readonly Mock<ITrilhaUsuarioUseCase> _trilhaUsuarioUseCaseMock;
        private readonly Mock<ITrilhaUseCase> _trilhaUseCaseMock;
        private readonly TrilhaUsuarioController _controller;
        private readonly Mock<IUrlHelper> _urlHelperMock;

        public TrilhaUsuarioControllerTests()
        {
            _trilhaUsuarioUseCaseMock = new Mock<ITrilhaUsuarioUseCase>();
            _trilhaUseCaseMock = new Mock<ITrilhaUseCase>();
            _urlHelperMock = new Mock<IUrlHelper>();
            
            _controller = new TrilhaUsuarioController(
                new MockLogger<TrilhaUsuarioController>(),
                _trilhaUsuarioUseCaseMock.Object,
                _trilhaUseCaseMock.Object
            );

            // Mock Url.Action para sempre retornar uma URL válida
            _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                         .Returns("/api/v1/usuarios/usuario-1/trilhas");

            _controller.Url = _urlHelperMock.Object;
        }

        [Fact(DisplayName = "Deve retornar OK com lista paginada de trilhas do usuário")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarOkComPaginacao()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var trilhasUsuario = new List<TrilhaUsuario>
            {
                new TrilhaUsuario { IdTrilhaUsuario = "tu-1", IdUsuario = idUsuario, IdTrilha = "t-1", TrilhaConcluidaUsuario = "N" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-2", IdUsuario = idUsuario, IdTrilha = "t-2", TrilhaConcluidaUsuario = "S" }
            };

            var trilhas = new List<Trilha>
            {
                new Trilha { IdTrilha = "t-1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 5 },
                new Trilha { IdTrilha = "t-2", NomeTrilha = "Trilha 2", QuantidadeConteudoTrilha = 3 }
            };

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhasDoUsuario(idUsuario))
                                     .ReturnsAsync(trilhasUsuario);

            _trilhaUseCaseMock.Setup(x => x.PegarTodasAsTrilhas())
                             .ReturnsAsync(trilhas);

            // Act
            var result = await _controller.PegarTrilhasDoUsuario(idUsuario);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult.Value);
            var response = Assert.IsType<PaginacaoResponse<TrilhaUsuarioCompleta>>(okResult.Value);
            Assert.NotNull(response.Data);
        }

        [Fact(DisplayName = "Deve retornar OK com trilha específica do usuário")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarOkQuandoExistir()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";

            var trilhaUsuario = new TrilhaUsuario 
            { 
                IdTrilhaUsuario = "tu-1", 
                IdUsuario = idUsuario, 
                IdTrilha = idTrilha, 
                TrilhaConcluidaUsuario = "N" 
            };

            var trilha = new Trilha 
            { 
                IdTrilha = idTrilha, 
                NomeTrilha = "Trilha Teste", 
                QuantidadeConteudoTrilha = 5 
            };

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhaDoUsuario(idUsuario, idTrilha))
                                     .ReturnsAsync(trilhaUsuario);

            _trilhaUseCaseMock.Setup(x => x.PegarTrilha(idTrilha))
                             .ReturnsAsync(trilha);

            // Act
            var result = await _controller.PegarTrilhaDoUsuario(idUsuario, idTrilha);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult.Value);
            var response = Assert.IsType<HateoasResponse<TrilhaUsuarioCompleta>>(okResult.Value);
            Assert.NotNull(response.Data);
        }

        [Fact(DisplayName = "Deve retornar NotFound quando trilha não existir")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarNotFoundQuandoNaoExistir()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-inexistente";

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhaDoUsuario(idUsuario, idTrilha))
                                     .ReturnsAsync((TrilhaUsuario)null!);

            _trilhaUseCaseMock.Setup(x => x.PegarTrilha(idTrilha))
                             .ReturnsAsync((Trilha)null!);

            // Act
            var result = await _controller.PegarTrilhaDoUsuario(idUsuario, idTrilha);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Deve retornar NotFound quando trilhaUsuario for nulo")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarNotFoundQuandoTrilhaUsuarioNulo()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";

            var trilha = new Trilha 
            { 
                IdTrilha = idTrilha, 
                NomeTrilha = "Trilha Teste", 
                QuantidadeConteudoTrilha = 5 
            };

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhaDoUsuario(idUsuario, idTrilha))
                                     .ReturnsAsync((TrilhaUsuario)null!);

            _trilhaUseCaseMock.Setup(x => x.PegarTrilha(idTrilha))
                             .ReturnsAsync(trilha);

            // Act
            var result = await _controller.PegarTrilhaDoUsuario(idUsuario, idTrilha);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact(DisplayName = "Deve retornar StatusCode 500 quando ocorrer erro")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarStatusCode500QuandoErro()
        {
            // Arrange
            var idUsuario = "usuario-1";

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhasDoUsuario(idUsuario))
                                     .ThrowsAsync(new Exception("Erro no repositório"));

            // Act
            var result = await _controller.PegarTrilhasDoUsuario(idUsuario);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = (ObjectResult)result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact(DisplayName = "Deve incluir links HATEOAS na resposta")]
        public async Task PegarTrilhaDoUsuario_DeveIncluirLinksHateoas()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var idTrilha = "trilha-1";

            var trilhaUsuario = new TrilhaUsuario 
            { 
                IdTrilhaUsuario = "tu-1", 
                IdUsuario = idUsuario, 
                IdTrilha = idTrilha, 
                TrilhaConcluidaUsuario = "N" 
            };

            var trilha = new Trilha 
            { 
                IdTrilha = idTrilha, 
                NomeTrilha = "Trilha Teste", 
                QuantidadeConteudoTrilha = 5 
            };

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhaDoUsuario(idUsuario, idTrilha))
                                     .ReturnsAsync(trilhaUsuario);

            _trilhaUseCaseMock.Setup(x => x.PegarTrilha(idTrilha))
                             .ReturnsAsync(trilha);

            // Act
            var result = await _controller.PegarTrilhaDoUsuario(idUsuario, idTrilha);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var response = Assert.IsType<HateoasResponse<TrilhaUsuarioCompleta>>(okResult.Value);
            Assert.NotNull(response.Links);
            // Verificar que existem links (Url.Action mockado retorna URL válida)
            Assert.True(response.Links.Count >= 3, "Deve ter ao menos 3 links (self, trilhasDoUsuario, conteudosDaTrilha)");
        }

        [Fact(DisplayName = "Deve retornar paginação correta")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarPaginacaoCorreta()
        {
            // Arrange
            var idUsuario = "usuario-1";
            var trilhasUsuario = new List<TrilhaUsuario>
            {
                new TrilhaUsuario { IdTrilhaUsuario = "tu-1", IdUsuario = idUsuario, IdTrilha = "t-1", TrilhaConcluidaUsuario = "N" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-2", IdUsuario = idUsuario, IdTrilha = "t-2", TrilhaConcluidaUsuario = "N" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-3", IdUsuario = idUsuario, IdTrilha = "t-3", TrilhaConcluidaUsuario = "N" }
            };

            var trilhas = new List<Trilha>
            {
                new Trilha { IdTrilha = "t-1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 5 },
                new Trilha { IdTrilha = "t-2", NomeTrilha = "Trilha 2", QuantidadeConteudoTrilha = 3 },
                new Trilha { IdTrilha = "t-3", NomeTrilha = "Trilha 3", QuantidadeConteudoTrilha = 4 }
            };

            _trilhaUsuarioUseCaseMock.Setup(x => x.PegarTrilhasDoUsuario(idUsuario))
                                     .ReturnsAsync(trilhasUsuario);

            _trilhaUseCaseMock.Setup(x => x.PegarTodasAsTrilhas())
                             .ReturnsAsync(trilhas);

            // Act - Pagina 1 com tamanho 2
            var result = await _controller.PegarTrilhasDoUsuario(idUsuario, 1, 2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var response = Assert.IsType<PaginacaoResponse<TrilhaUsuarioCompleta>>(okResult.Value);
            Assert.Equal(1, response.PaginaAtual);
            Assert.Equal(2, response.TamanhoPagina);
            Assert.Equal(3, response.TotalItens);
        }
    }
}
