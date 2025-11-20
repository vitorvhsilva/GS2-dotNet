using API.Domain.Entities;
using API.Infraestructure.Data.AppData;
using API.Infraestructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.Infrastructure
{
    public class ConteudoTrilhaUsuarioRepositoryTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact(DisplayName = "Deve retornar lista de conteúdos da trilha do usuário")]
        public async Task PegarTodasOsConteudosTrilhaUsuario_DeveRetornarLista()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var usuario = new Usuario 
            { 
                IdUsuario = "usuario-1", 
                NomeUsuario = "Usuário Teste", 
                EmailUsuario = "teste@email.com", 
                SenhaUsuario = "senha",
                DataNascimentoUsuario = DateTime.Now
            };
            
            var trilha = new Trilha { IdTrilha = "trilha-1", NomeTrilha = "Trilha Teste", QuantidadeConteudoTrilha = 2 };
            
            var conteudo1 = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "c-1", 
                NomeConteudoTrilha = "Conteúdo 1", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = "trilha-1" 
            };
            
            var conteudo2 = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "c-2", 
                NomeConteudoTrilha = "Conteúdo 2", 
                TipoConteudoTrilha = "Artigo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = "trilha-1" 
            };
            
            var conteudosUsuario = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = "usuario-1", IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S", ConteudoTrilha = conteudo1 },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-2", IdUsuario = "usuario-1", IdConteudoTrilha = "c-2", ConteudoTrilhaConcluidaUsuario = "N", ConteudoTrilha = conteudo2 }
            };

            context.Usuarios.Add(usuario);
            context.Trilhas.Add(trilha);
            context.ConteudosTrilha.AddRange(conteudo1, conteudo2);
            context.ConteudosTrilhaUsuario.AddRange(conteudosUsuario);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarTodasOsConteudosTrilhaUsuario("usuario-1", "trilha-1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "Deve retornar conteúdo da trilha do usuário por ID")]
        public async Task PegarConteudoTrilhaUsuario_DeveRetornarConteudoQuandoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var usuario = new Usuario 
            { 
                IdUsuario = "usuario-1", 
                NomeUsuario = "Usuário Teste", 
                EmailUsuario = "teste@email.com", 
                SenhaUsuario = "senha",
                DataNascimentoUsuario = DateTime.Now
            };
            
            var conteudoUsuario = new ConteudoTrilhaUsuario 
            { 
                IdConteudoTrilhaUsuario = "ctu-1", 
                IdUsuario = "usuario-1", 
                IdConteudoTrilha = "conteudo-123", 
                ConteudoTrilhaConcluidaUsuario = "N" 
            };

            context.Usuarios.Add(usuario);
            context.ConteudosTrilhaUsuario.Add(conteudoUsuario);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarConteudoTrilhaUsuario("usuario-1", "conteudo-123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("conteudo-123", result.IdConteudoTrilha);
            Assert.Equal("usuario-1", result.IdUsuario);
        }

        [Fact(DisplayName = "Deve concluir conteúdo da trilha do usuário")]
        public async Task ConcluirConteudoTrilhaUsuario_DeveConcluir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var usuario = new Usuario 
            { 
                IdUsuario = "usuario-1", 
                NomeUsuario = "Usuário Teste", 
                EmailUsuario = "teste@email.com", 
                SenhaUsuario = "senha",
                DataNascimentoUsuario = DateTime.Now
            };
            
            var conteudoUsuario = new ConteudoTrilhaUsuario 
            { 
                IdConteudoTrilhaUsuario = "ctu-1", 
                IdUsuario = "usuario-1", 
                IdConteudoTrilha = "conteudo-123", 
                ConteudoTrilhaConcluidaUsuario = "N" 
            };

            context.Usuarios.Add(usuario);
            context.ConteudosTrilhaUsuario.Add(conteudoUsuario);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaUsuarioRepository(context);

            // Act
            await repository.ConcluirConteudoTrilhaUsuario("usuario-1", "conteudo-123");

            // Assert
            var result = await repository.PegarConteudoTrilhaUsuario("usuario-1", "conteudo-123");
            Assert.NotNull(result);
            Assert.Equal("S", result.ConteudoTrilhaConcluidaUsuario);
        }

        [Fact(DisplayName = "Deve retornar nulo quando conteúdo do usuário não existir")]
        public async Task PegarConteudoTrilhaUsuario_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new ConteudoTrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarConteudoTrilhaUsuario("usuario-inexistente", "conteudo-inexistente");

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando não houver conteúdos do usuário na trilha")]
        public async Task PegarTodasOsConteudosTrilhaUsuario_DeveRetornarListaVaziaQuandoNaoHouver()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var usuario = new Usuario 
            { 
                IdUsuario = "usuario-1", 
                NomeUsuario = "Usuário Teste", 
                EmailUsuario = "teste@email.com", 
                SenhaUsuario = "senha",
                DataNascimentoUsuario = DateTime.Now
            };
            
            var trilha = new Trilha { IdTrilha = "trilha-1", NomeTrilha = "Trilha Teste", QuantidadeConteudoTrilha = 0 };

            context.Usuarios.Add(usuario);
            context.Trilhas.Add(trilha);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarTodasOsConteudosTrilhaUsuario("usuario-1", "trilha-1");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Deve filtrar conteúdos por usuário e trilha corretamente")]
        public async Task PegarTodasOsConteudosTrilhaUsuario_DeveFiltrarCorretamente()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var usuario1 = new Usuario 
            { 
                IdUsuario = "usuario-1", 
                NomeUsuario = "Usuário 1", 
                EmailUsuario = "teste1@email.com", 
                SenhaUsuario = "senha",
                DataNascimentoUsuario = DateTime.Now
            };
            
            var usuario2 = new Usuario 
            { 
                IdUsuario = "usuario-2", 
                NomeUsuario = "Usuário 2", 
                EmailUsuario = "teste2@email.com", 
                SenhaUsuario = "senha",
                DataNascimentoUsuario = DateTime.Now
            };
            
            var trilha1 = new Trilha { IdTrilha = "trilha-1", NomeTrilha = "Trilha 1", QuantidadeConteudoTrilha = 1 };
            var trilha2 = new Trilha { IdTrilha = "trilha-2", NomeTrilha = "Trilha 2", QuantidadeConteudoTrilha = 1 };
            
            var conteudo1 = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "c-1", 
                NomeConteudoTrilha = "Conteúdo 1", 
                TipoConteudoTrilha = "Vídeo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = "trilha-1" 
            };
            
            var conteudo2 = new ConteudoTrilha 
            { 
                IdConteudoTrilha = "c-2", 
                NomeConteudoTrilha = "Conteúdo 2", 
                TipoConteudoTrilha = "Artigo", 
                TextoConteudoTrilha = "Texto",
                IdTrilha = "trilha-2" 
            };
            
            var conteudosUsuario = new List<ConteudoTrilhaUsuario>
            {
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-1", IdUsuario = "usuario-1", IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "S", ConteudoTrilha = conteudo1 },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-2", IdUsuario = "usuario-1", IdConteudoTrilha = "c-2", ConteudoTrilhaConcluidaUsuario = "N", ConteudoTrilha = conteudo2 },
                new ConteudoTrilhaUsuario { IdConteudoTrilhaUsuario = "ctu-3", IdUsuario = "usuario-2", IdConteudoTrilha = "c-1", ConteudoTrilhaConcluidaUsuario = "N", ConteudoTrilha = conteudo1 }
            };

            context.Usuarios.AddRange(usuario1, usuario2);
            context.Trilhas.AddRange(trilha1, trilha2);
            context.ConteudosTrilha.AddRange(conteudo1, conteudo2);
            context.ConteudosTrilhaUsuario.AddRange(conteudosUsuario);
            await context.SaveChangesAsync();

            var repository = new ConteudoTrilhaUsuarioRepository(context);

            // Act
            var resultUsuario1Trilha1 = await repository.PegarTodasOsConteudosTrilhaUsuario("usuario-1", "trilha-1");
            var resultUsuario1Trilha2 = await repository.PegarTodasOsConteudosTrilhaUsuario("usuario-1", "trilha-2");
            var resultUsuario2Trilha1 = await repository.PegarTodasOsConteudosTrilhaUsuario("usuario-2", "trilha-1");

            // Assert
            Assert.Single(resultUsuario1Trilha1);
            Assert.Single(resultUsuario1Trilha2);
            Assert.Single(resultUsuario2Trilha1);
        }
    }
}
