using API.Application.Interface;
using API.Application.Service;
using API.Application.Util;
using API.Domain.Entities;
using API.Domain.Interface;
using Moq;
using Tests.UnitTest;
using Xunit;

namespace Tests.UnitTest.Application
{
    public class ConteudoTrilhaUsuarioUseCaseTests
    {
        private readonly Mock<IConteudoTrilhaUsuarioRepository> _conteudoRepositoryMock;
        private readonly Mock<ITrilhaUsuarioRepository> _trilhaRepositoryMock;
        private readonly ConteudoTrilhaUsuarioUseCase _useCase;

        public ConteudoTrilhaUsuarioUseCaseTests()
        {
            _conteudoRepositoryMock = new Mock<IConteudoTrilhaUsuarioRepository>();
            _trilhaRepositoryMock = new Mock<ITrilhaUsuarioRepository>();
            _useCase = new ConteudoTrilhaUsuarioUseCase(new MockLogger<ConteudoTrilhaUsuarioUseCase>(), _conteudoRepositoryMock.Object, _trilhaRepositoryMock.Object);
        }

        [Fact(DisplayName = "Deve retornar lista de conteúdos da trilha do usuário")]
        public async Task PegarTodasOsConteudosTrilhaUsuario_DeveRetornarLista()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var idTrilha = "trilha-456";
            var conteudos = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = idUsuario, IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S" },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-2", IdUsuario = idUsuario, IdConteudoTrilha = "c-2", ConteudoTrilhaConcluidaUsuario = "N" }
            };

            _conteudoRepositoryMock.Setup(r => r.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha))
                                   .ReturnsAsync(conteudos);

            // Act
            var result = await _useCase.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _conteudoRepositoryMock.Verify(r => r.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar conteúdo da trilha do usuário por ID")]
        public async Task PegarConteudoTrilhaUsuario_DeveRetornarConteudoQuandoExistir()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var idConteudo = "conteudo-456";
            var conteudoUsuario = new ConteudoTrilhaUsuario 
            { 
                IdConteudoTrilhaUsuario = "ctu-1", 
                IdUsuario = idUsuario, 
                IdConteudoTrilha = idConteudo, 
                ConteudoTrilhaConcluidaUsuario = "N" 
            };

            _conteudoRepositoryMock.Setup(r => r.PegarConteudoTrilhaUsuario(idUsuario, idConteudo))
                                   .ReturnsAsync(conteudoUsuario);

            // Act
            var result = await _useCase.PegarConteudoTrilhaUsuario(idUsuario, idConteudo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idConteudo, result.IdConteudoTrilha);
            _conteudoRepositoryMock.Verify(r => r.PegarConteudoTrilhaUsuario(idUsuario, idConteudo), Times.Once);
        }

        [Fact(DisplayName = "Deve concluir conteúdo da trilha do usuário")]
        public async Task ConcluirConteudoTrilhaUsuario_DeveConcluir()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var idTrilha = "trilha-456";
            var idConteudo = "conteudo-789";

            var conteudosRestantes = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = idUsuario, IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S" }
            };

            _conteudoRepositoryMock.Setup(r => r.ConcluirConteudoTrilhaUsuario(idUsuario, idConteudo))
                                   .Returns(Task.CompletedTask);

            _conteudoRepositoryMock.Setup(r => r.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha))
                                   .ReturnsAsync(conteudosRestantes);

            // Act
            await _useCase.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            _conteudoRepositoryMock.Verify(r => r.ConcluirConteudoTrilhaUsuario(idUsuario, idConteudo), Times.Once);
        }

        [Fact(DisplayName = "Deve concluir trilha quando todos os conteúdos estiverem concluídos")]
        public async Task ConcluirConteudoTrilhaUsuario_DeveConcluirTrilhaQuandoCompleta()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var idTrilha = "trilha-456";
            var idConteudo = "conteudo-789";

            // Todos os conteúdos concluídos
            var conteudosCompletos = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = idUsuario, IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S" },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-2", IdUsuario = idUsuario, IdConteudoTrilha = "c-2", ConteudoTrilhaConcluidaUsuario = "S" }
            };

            _conteudoRepositoryMock.Setup(r => r.ConcluirConteudoTrilhaUsuario(idUsuario, idConteudo))
                                   .Returns(Task.CompletedTask);

            _conteudoRepositoryMock.Setup(r => r.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha))
                                   .ReturnsAsync(conteudosCompletos);

            _trilhaRepositoryMock.Setup(r => r.ConcluirTrilhaDoUsuario(idUsuario, idTrilha))
                                .Returns(Task.CompletedTask);

            // Act
            await _useCase.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            _conteudoRepositoryMock.Verify(r => r.ConcluirConteudoTrilhaUsuario(idUsuario, idConteudo), Times.Once);
            _trilhaRepositoryMock.Verify(r => r.ConcluirTrilhaDoUsuario(idUsuario, idTrilha), Times.Once);
        }

        [Fact(DisplayName = "Não deve concluir trilha quando houver conteúdos incompletos")]
        public async Task ConcluirConteudoTrilhaUsuario_NaoDeveConcluirTrilhaQuandoHouverIncompletos()
        {
            // Arrange
            var idUsuario = "usuario-123";
            var idTrilha = "trilha-456";
            var idConteudo = "conteudo-789";

            // Ainda há conteúdos não concluídos
            var conteudosMistos = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = idUsuario, IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S" },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-2", IdUsuario = idUsuario, IdConteudoTrilha = "c-2", ConteudoTrilhaConcluidaUsuario = "N" }
            };

            _conteudoRepositoryMock.Setup(r => r.ConcluirConteudoTrilhaUsuario(idUsuario, idConteudo))
                                   .Returns(Task.CompletedTask);

            _conteudoRepositoryMock.Setup(r => r.PegarTodasOsConteudosTrilhaUsuario(idUsuario, idTrilha))
                                   .ReturnsAsync(conteudosMistos);

            // Act
            await _useCase.ConcluirConteudoTrilhaUsuario(idUsuario, idTrilha, idConteudo);

            // Assert
            _conteudoRepositoryMock.Verify(r => r.ConcluirConteudoTrilhaUsuario(idUsuario, idConteudo), Times.Once);
            _trilhaRepositoryMock.Verify(r => r.ConcluirTrilhaDoUsuario(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar nulo quando conteúdo do usuário não existir")]
        public async Task PegarConteudoTrilhaUsuario_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            _conteudoRepositoryMock.Setup(r => r.PegarConteudoTrilhaUsuario(It.IsAny<string>(), It.IsAny<string>()))
                                   .ReturnsAsync((ConteudoTrilhaUsuario)null!);

            // Act
            var result = await _useCase.PegarConteudoTrilhaUsuario("usuario-inexistente", "conteudo-inexistente");

            // Assert
            Assert.Null(result);
            _conteudoRepositoryMock.Verify(r => r.PegarConteudoTrilhaUsuario(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
