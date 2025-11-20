using API.Domain.Entities;
using API.Infraestructure.Data.AppData;
using API.Infraestructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.UnitTests.Infrastructure
{
    public class TrilhaUsuarioRepositoryTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact(DisplayName = "Deve retornar trilhas do usuário")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarLista()
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
            
            var trilhas = new List<TrilhaUsuario>
            {
                new TrilhaUsuario { IdTrilhaUsuario = "tu-1", IdUsuario = "usuario-1", IdTrilha = "t-1", TrilhaConcluidaUsuario = "N" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-2", IdUsuario = "usuario-1", IdTrilha = "t-2", TrilhaConcluidaUsuario = "S" }
            };

            context.Usuarios.Add(usuario);
            context.TrilhasUsuarios.AddRange(trilhas);
            await context.SaveChangesAsync();

            var repository = new TrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarTrilhasDoUsuario("usuario-1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact(DisplayName = "Deve retornar trilha do usuário por ID")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarTrilhaQuandoExistir()
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
            
            var trilhaUsuario = new TrilhaUsuario 
            { 
                IdTrilhaUsuario = "tu-1", 
                IdUsuario = "usuario-1", 
                IdTrilha = "trilha-1", 
                TrilhaConcluidaUsuario = "N" 
            };

            context.Usuarios.Add(usuario);
            context.TrilhasUsuarios.Add(trilhaUsuario);
            await context.SaveChangesAsync();

            var repository = new TrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarTrilhaDoUsuario("usuario-1", "trilha-1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("trilha-1", result.IdTrilha);
            Assert.Equal("usuario-1", result.IdUsuario);
        }

        [Fact(DisplayName = "Deve retornar nulo quando trilha do usuário não existir")]
        public async Task PegarTrilhaDoUsuario_DeveRetornarNuloQuandoNaoExistir()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new TrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarTrilhaDoUsuario("usuario-inexistente", "trilha-inexistente");

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve concluir trilha do usuário")]
        public async Task ConcluirTrilhaDoUsuario_DeveConcluir()
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
            
            var trilhaUsuario = new TrilhaUsuario 
            { 
                IdTrilhaUsuario = "tu-1", 
                IdUsuario = "usuario-1", 
                IdTrilha = "trilha-1", 
                TrilhaConcluidaUsuario = "N" 
            };

            context.Usuarios.Add(usuario);
            context.TrilhasUsuarios.Add(trilhaUsuario);
            await context.SaveChangesAsync();

            var repository = new TrilhaUsuarioRepository(context);

            // Act
            await repository.ConcluirTrilhaDoUsuario("usuario-1", "trilha-1");

            // Assert
            var result = await repository.PegarTrilhaDoUsuario("usuario-1", "trilha-1");
            Assert.NotNull(result);
            Assert.Equal("S", result.TrilhaConcluidaUsuario);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando usuário não possuir trilhas")]
        public async Task PegarTrilhasDoUsuario_DeveRetornarListaVaziaQuandoNaoHouver()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new TrilhaUsuarioRepository(context);

            // Act
            var result = await repository.PegarTrilhasDoUsuario("usuario-sem-trilhas");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact(DisplayName = "Deve filtrar trilhas por usuário corretamente")]
        public async Task PegarTrilhasDoUsuario_DeveFiltrarPorUsuarioCorretamente()
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

            var trilhas = new List<TrilhaUsuario>
            {
                new TrilhaUsuario { IdTrilhaUsuario = "tu-1", IdUsuario = "usuario-1", IdTrilha = "t-1", TrilhaConcluidaUsuario = "N" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-2", IdUsuario = "usuario-1", IdTrilha = "t-2", TrilhaConcluidaUsuario = "S" },
                new TrilhaUsuario { IdTrilhaUsuario = "tu-3", IdUsuario = "usuario-2", IdTrilha = "t-1", TrilhaConcluidaUsuario = "N" }
            };

            context.Usuarios.AddRange(usuario1, usuario2);
            context.TrilhasUsuarios.AddRange(trilhas);
            await context.SaveChangesAsync();

            var repository = new TrilhaUsuarioRepository(context);

            // Act
            var resultUsuario1 = await repository.PegarTrilhasDoUsuario("usuario-1");
            var resultUsuario2 = await repository.PegarTrilhasDoUsuario("usuario-2");

            // Assert
            Assert.Equal(2, resultUsuario1.Count());
            Assert.Single(resultUsuario2);
        }
    }
}
