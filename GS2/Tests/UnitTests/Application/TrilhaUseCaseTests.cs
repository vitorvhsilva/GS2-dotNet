using API.Application.Service;
using API.Domain.Entities;
using API.Domain.Interface;
using Moq;
using Tests.UnitTest;
using Xunit;

namespace Tests.UnitTest.Application
{
    public class TrilhaUseCaseTests
    {
        private readonly Mock<ITrilhaRepository> _repositoryMock;
        private readonly TrilhaUseCase _useCase;

        public TrilhaUseCaseTests()
        {
            _repositoryMock = new Mock<ITrilhaRepository>();
            _useCase = new TrilhaUseCase(_repositoryMock.Object, new MockLogger<TrilhaUseCase>());
        }

        [Fact(DisplayName = "Deve retornar lista de todas as trilhas")]
        public async Task PegarTodasAsTrilhas_DeveRetornarLista()
        {
            // Arrange
            var trilhas = new List<Trilha>
            {
                new Trilha { IdTrilha = "1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 5 },
                new Trilha { IdTrilha = "2", NomeTrilha = "Trilha 2", QuantidadeConteudoTrilha = 3 }
            };

            _repositoryMock.Setup(r => r.PegarTodasAsTrilhas())
                           .ReturnsAsync(trilhas);

            // Act
            var result = await _useCase.PegarTodasAsTrilhas();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _repositoryMock.Verify(r => r.PegarTodasAsTrilhas(), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar trilha por ID")]
        public async Task PegarTrilha_DeveRetornarTrilhaQuandoExistir()
        {
            // Arrange
            var trilha = new Trilha { IdTrilha = "1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 5 };

            _repositoryMock.Setup(r => r.PegarTrilha("1"))
                           .ReturnsAsync(trilha);

            // Act
            var result = await _useCase.PegarTrilha("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.IdTrilha);
            Assert.Equal("Trilha 1", result.NomeTrilha);
            _repositoryMock.Verify(r => r.PegarTrilha("1"), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar nulo quando trilha não existir")]
        public async Task PegarTrilha_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            _repositoryMock.Setup(r => r.PegarTrilha(It.IsAny<string>()))
                           .ReturnsAsync((Trilha)null!);

            // Act
            var result = await _useCase.PegarTrilha("id-inexistente");

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.PegarTrilha("id-inexistente"), Times.Once);
        }

        [Fact(DisplayName = "Deve chamar repositório corretamente com parâmetro correto")]
        public async Task PegarTrilha_DeveChamarRepositorioComIdCorreto()
        {
            // Arrange
            var idTrilha = "trilha-123";
            _repositoryMock.Setup(r => r.PegarTrilha(idTrilha))
                           .ReturnsAsync(new Trilha { IdTrilha = idTrilha });

            // Act
            await _useCase.PegarTrilha(idTrilha);

            // Assert
            _repositoryMock.Verify(r => r.PegarTrilha(idTrilha), Times.Once);
        }
    }
}
