using API.Application.Service;
using API.Domain.Entities;
using API.Domain.Interface;
using Moq;
using Tests.UnitTest;
using Xunit;

namespace Tests.UnitTest.Application
{
    public class ConteudoTrilhaUseCaseTests
    {
        private readonly Mock<IConteudoTrilhaRepository> _repositoryMock;
        private readonly ConteudoTrilhaUseCase _useCase;

        public ConteudoTrilhaUseCaseTests()
        {
            _repositoryMock = new Mock<IConteudoTrilhaRepository>();
            _useCase = new ConteudoTrilhaUseCase(new MockLogger<ConteudoTrilhaUseCase>(), _repositoryMock.Object);
        }

        [Fact(DisplayName = "Deve retornar lista de conteúdos da trilha")]
        public async Task PegarTodasOsConteudosTrilha_DeveRetornarLista()
        {
            // Arrange
            var idTrilha = "trilha-123";
            var conteudos = new List<ConteudoTrilha>
            {
                new ConteudoTrilha { IdConteudoTrilha = "c-1", NomeConteudoTrilha = "Conteúdo 1", TipoConteudoTrilha = "Vídeo", IdTrilha = idTrilha },
                new ConteudoTrilha { IdConteudoTrilha = "c-2", NomeConteudoTrilha = "Conteúdo 2", TipoConteudoTrilha = "Artigo", IdTrilha = idTrilha }
            };

            _repositoryMock.Setup(r => r.PegarTodasOsConteudosTrilha(idTrilha))
                           .ReturnsAsync(conteudos);

            // Act
            var result = await _useCase.PegarTodasOsConteudosTrilha(idTrilha);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _repositoryMock.Verify(r => r.PegarTodasOsConteudosTrilha(idTrilha), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar conteúdo por ID")]
        public async Task PegarConteudoTrilha_DeveRetornarConteudoQuandoExistir()
        {
            // Arrange
            var conteudo = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "c-1", 
                NomeConteudoTrilha = "Conteúdo 1", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Texto do conteúdo",
                IdTrilha = "t-1" 
            };

            _repositoryMock.Setup(r => r.PegarConteudoTrilha("c-1"))
                           .ReturnsAsync(conteudo);

            // Act
            var result = await _useCase.PegarConteudoTrilha("c-1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("c-1", result.IdConteudoTrilha);
            Assert.Equal("Conteúdo 1", result.NomeConteudoTrilha);
            _repositoryMock.Verify(r => r.PegarConteudoTrilha("c-1"), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar nulo quando conteúdo não existir")]
        public async Task PegarConteudoTrilha_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            _repositoryMock.Setup(r => r.PegarConteudoTrilha(It.IsAny<string>()))
                           .ReturnsAsync((ConteudoTrilha)null!);

            // Act
            var result = await _useCase.PegarConteudoTrilha("id-inexistente");

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(r => r.PegarConteudoTrilha("id-inexistente"), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando trilha não possuir conteúdos")]
        public async Task PegarTodasOsConteudosTrilha_DeveRetornarListaVaziaQuandoNaoHouver()
        {
            // Arrange
            var idTrilha = "trilha-sem-conteudo";
            _repositoryMock.Setup(r => r.PegarTodasOsConteudosTrilha(idTrilha))
                           .ReturnsAsync(new List<ConteudoTrilha>());

            // Act
            var result = await _useCase.PegarTodasOsConteudosTrilha(idTrilha);

            // Assert
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.PegarTodasOsConteudosTrilha(idTrilha), Times.Once);
        }
    }
}
