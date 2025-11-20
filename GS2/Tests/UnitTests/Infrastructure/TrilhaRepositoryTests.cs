using API.Domain.Entities;
using API.Infraestructure.Data.AppData;
using API.Infraestructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.Infrastructure
{
    public class TrilhaRepositoryTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact(DisplayName = "Deve retornar todas as trilhas")]
        public async Task PegarTodasAsTrilhas_DeveRetornarLista()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilhas = new List<Trilha>
            {
                new Trilha { IdTrilha = "1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 5 },
                new Trilha { IdTrilha = "2", NomeTrilha = "Trilha 2", QuantidadeConteudoTrilha = 3 }
            };

            context.Trilhas.AddRange(trilhas);
            await context.SaveChangesAsync();

            var repository = new TrilhaRepository(context);

            // Act
            var result = await repository.PegarTodasAsTrilhas();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "Deve retornar uma trilha por ID")]
        public async Task PegarTrilha_DeveRetornarTrilhaQuandoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilha = new Trilha { IdTrilha = "trilha-123", NomeTrilha = "Trilha Teste", QuantidadeConteudoTrilha = 5 };

            context.Trilhas.Add(trilha);
            await context.SaveChangesAsync();

            var repository = new TrilhaRepository(context);

            // Act
            var result = await repository.PegarTrilha("trilha-123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("trilha-123", result.IdTrilha);
            Assert.Equal("Trilha Teste", result.NomeTrilha);
        }

        [Fact(DisplayName = "Deve retornar nulo quando trilha não existir")]
        public async Task PegarTrilha_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new TrilhaRepository(context);

            // Act
            var result = await repository.PegarTrilha("id-inexistente");

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando não houver trilhas")]
        public async Task PegarTodasAsTrilhas_DeveRetornarListaVazia()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new TrilhaRepository(context);

            // Act
            var result = await repository.PegarTodasAsTrilhas();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Deve filtrar trilhas corretamente")]
        public async Task PegarTodasAsTrilhas_DeveFiltrarCorretamente()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilhas = new List<Trilha>
            {
                new Trilha { IdTrilha = "1", NomeTrilha = "Trilha A", QuantidadeConteudoTrilha = 5 },
                new Trilha { IdTrilha = "2", NomeTrilha = "Trilha B", QuantidadeConteudoTrilha = 3 },
                new Trilha { IdTrilha = "3", NomeTrilha = "Trilha C", QuantidadeConteudoTrilha = 7 }
            };

            context.Trilhas.AddRange(trilhas);
            await context.SaveChangesAsync();

            var repository = new TrilhaRepository(context);

            // Act
            var result = await repository.PegarTrilha("2");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Trilha B", result.NomeTrilha);
            Assert.Equal(3, result.QuantidadeConteudoTrilha);
        }
    }
}
