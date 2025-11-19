using API.Application.Service;
using API.Domain.Entities;
using API.Domain.Interface;
using Moq;
using Tests.UnitTest;
using Xunit;

namespace Tests.UnitTest.Application
{
    public class TrilhaUsuarioUseCaseTests
    {
        private readonly Mock<ITrilhaUsuarioRepository> _repositoryMock;
        private readonly TrilhaUsuarioUseCase _useCase;

        public TrilhaUsuarioUseCaseTests()
        {
            _repositoryMock = new Mock<ITrilhaUsuarioRepository>();
            _useCase = new TrilhaUsuarioUseCase(_repositoryMock.Object, new MockLogger<TrilhaUsuarioUseCase>());
        }

        [Fact(DisplayName = "Deve retornar lista de trilhas do usuário")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarLista()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var trilhasUsuario = new List<TrilhaUsuario>
            {
                new TrilhaUsuario { IdTrilhaUsuario = "tu-1", IdUsuario = idUsuario, IdTrilha = "t-1", TrilhaConcluidaUsuario = "N" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-2", IdUsuario = idUsuario, IdTrilha = "t-2", TrilhaConcluidaUsuario = "S" }
            };

            _repositoryMock.Setup(r => r.PegarTrilhasDoUsuario(idUsuario))
                           .ReturnsAsync(trilhasUsuario);

            // Act
            var result = await _useCase.PegarTrilhasDoUsuario(idUsuario);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _repositoryMock.Verify(r => r.PegarTrilhasDoUsuario(idUsuario), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar trilha do usuário por ID")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarTrilhaQuandoExistir()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var idTrilha = "trilha-456";
            var trilhaUsuario = new TrilhaUsuario 
            { 
                IdTrilhaUsuario = "tu-1", 
                IdUsuario = idUsuario, 
                IdTrilha = idTrilha, 
                TrilhaConcluidaUsuario = "N" 
            };

            _repositoryMock.Setup(r => r.PegarTrilhaDoUsuario(idUsuario, idTrilha))
                           .ReturnsAsync(trilhaUsuario);

            // Act
            var result = await _useCase.PegarTrilhaDoUsuario(idUsuario, idTrilha);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idTrilha, result.IdTrilha);
            Assert.Equal(idUsuario, result.IdUsuario);
            _repositoryMock.Verify(r => r.PegarTrilhaDoUsuario(idUsuario, idTrilha), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar nulo quando trilha do usuário não existir")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            _repositoryMock.Setup(r => r.PegarTrilhaDoUsuario(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync((TrilhaUsuario)null!);

            // Act
            var result = await _useCase.PegarTrilhaDoUsuario("usuario-inexistente", "trilha-inexistente");

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.PegarTrilhaDoUsuario(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando usuário não possuir trilhas")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarListaVaziaQuandoNaoHouver()
        {
            // Arrange
            var idUsuario = "usuario-sem-trilhas";
            _repositoryMock.Setup(r => r.PegarTrilhasDoUsuario(idUsuario))
                           .ReturnsAsync(new List<TrilhaUsuario>());

            // Act
            var result = await _useCase.PegarTrilhasDoUsuario(idUsuario);

            // Assert
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.PegarTrilhasDoUsuario(idUsuario), Times.Once);
        }
    }
}
