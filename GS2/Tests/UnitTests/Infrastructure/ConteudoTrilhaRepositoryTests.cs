using API.Domain.Entities;
using API.Infraestructure.Data.AppData;
using API.Infraestructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.Infrastructure
{
    public class ConteudoTrilhaRepositoryTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact(DisplayName = "Deve retornar lista de conteúdos da trilha")]
        public async Task PegarTodasOsConteudosTrilha_DeveRetornarLista()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilha = new Trilha { IdTrilha = "trilha-1", NomeTrilha = "Trilha Teste", QuantidadeConteudoTrilha = 2 };
            
            var conteudos = new List<ConteudoTrilha>
            {
                new ConteudoTrilha { IdConteudoTrilha = "c-1", NomeConteudoTrilha = "Conteúdo 1", TipoConteudoTrilha = "Vídeo", TextoConteudoTrilha = "Texto", IdTrilha = "trilha-1" },
                new ConteudoTrilha { IdConteudoTrilha = "c-2", NomeConteudoTrilha = "Conteúdo 2", TipoConteudoTrilha = "Artigo", TextoConteudoTrilha = "Texto", IdTrilha = "trilha-1" }
            };

            context.Trilhas.Add(trilha);
            context.ConteudosTrilha.AddRange(conteudos);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaRepository(context);

            // Act
            var result = await repository.PegarTodasOsConteudosTrilha("trilha-1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "Deve retornar conteúdo por ID")]
        public async Task PegarConteudoTrilha_DeveRetornarConteudoQuandoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilha = new Trilha { IdTrilha = "trilha-1", NomeTrilha = "Trilha Teste", QuantidadeConteudoTrilha = 1 };
            
            var conteudo = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "conteudo-123", 
                NomeConteudoTrilha = "Conteúdo Teste", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Descrição do conteúdo",
                IdTrilha = "trilha-1" 
            };

            context.Trilhas.Add(trilha);
            context.ConteudosTrilha.Add(conteudo);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaRepository(context);

            // Act
            var result = await repository.PegarConteudoTrilha("conteudo-123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("conteudo-123", result.IdConteudoTrilha);
            Assert.Equal("Conteúdo Teste", result.NomeConteudoTrilha);
        }

        [Fact(DisplayName = "Deve retornar nulo quando conteúdo não existir")]
        public async Task PegarConteudoTrilha_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new ConteudoTrilhaRepository(context);

            // Act
            var result = await repository.PegarConteudoTrilha("id-inexistente");

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando trilha não possuir conteúdos")]
        public async Task PegarTodasOsConteudosTrilha_DeveRetornarListaVaziaQuandoNaoHouver()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilha = new Trilha { IdTrilha = "trilha-vazia", NomeTrilha = "Trilha Vazia", QuantidadeConteudoTrilha = 0 };
            
            context.Trilhas.Add(trilha);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaRepository(context);

            // Act
            var result = await repository.PegarTodasOsConteudosTrilha("trilha-vazia");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Deve filtrar conteúdos por trilha corretamente")]
        public async Task PegarTodasOsConteudosTrilha_DeveFiltrarPorTrilhaCorretamente()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var trilha1 = new Trilha { IdTrilha = "trilha-1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 2 };
            var trilha2 = new Trilha { IdTrilha = "trilha-2", NomeTrilha = "Trilha 2", QuantidadeConteudoTrilha = 1 };
            
            var conteudos = new List<ConteudoTrilha>
            {
                new ConteudoTrilha { IdConteudoTrilha = "c-1", NomeConteudoTrilha = "Conteúdo 1", TipoConteudoTrilha = "Vídeo", TextoConteudoTrilha = "Texto", IdTrilha = "trilha-1" },
                new ConteudoTrilha { IdConteudoTrilha = "c-2", NomeConteudoTrilha = "Conteúdo 2", TipoConteudoTrilha = "Artigo", TextoConteudoTrilha = "Texto", IdTrilha = "trilha-1" },
                new ConteudoTrilha { IdConteudoTrilha = "c-3", NomeConteudoTrilha = "Conteúdo 3", TipoConteudoTrilha = "Vídeo", TextoConteudoTrilha = "Texto", IdTrilha = "trilha-2" }
            };

            context.Trilhas.AddRange(trilha1, trilha2);
            context.ConteudosTrilha.AddRange(conteudos);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaRepository(context);

            // Act
            var resultTrilha1 = await repository.PegarTodasOsConteudosTrilha("trilha-1");
            var resultTrilha2 = await repository.PegarTodasOsConteudosTrilha("trilha-2");

            // Assert
            Assert.Equal(2, resultTrilha1.Count());
            Assert.Single(resultTrilha2);
        }
    }
}
