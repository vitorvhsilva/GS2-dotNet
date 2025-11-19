# Testes Unitários das Use Cases

## Visão Geral
Todos os testes foram criados seguindo o padrão do exemplo fornecido, utilizando **Moq** para mocking de dependências e **xUnit** como framework de testes.

## Estrutura dos Testes

### 1. **TrilhaUseCaseTests** (`Tests/UnitTests/Application/TrilhaUseCaseTests.cs`)
Testa a use case `TrilhaUseCase` com as seguintes verificações:

- ? **PegarTodasAsTrilhas_DeveRetornarLista**: Verifica se a use case retorna uma lista de trilhas
- ? **PegarTrilha_DeveRetornarTrilhaQuandoExistir**: Verifica se retorna uma trilha específica por ID
- ? **PegarTrilha_DeveRetornarNuloQuandoNaoExistir**: Verifica se retorna nulo quando trilha não existe
- ? **PegarTrilha_DeveChamarRepositorioComIdCorreto**: Verifica se o repositório é chamado com parâmetros corretos

### 2. **TrilhaUsuarioUseCaseTests** (`Tests/UnitTests/Application/TrilhaUsuarioUseCaseTests.cs`)
Testa a use case `TrilhaUsuarioUseCase` com as seguintes verificações:

- ? **PegarTrilhasDoUsuario_DeveRetornarLista**: Verifica se retorna lista de trilhas do usuário
- ? **PegarTrilhaDoUsuario_DeveRetornarTrilhaQuandoExistir**: Verifica se retorna trilha específica do usuário
- ? **PegarTrilhaDoUsuario_DeveRetornarNuloQuandoNaoExistir**: Verifica se retorna nulo quando não existe
- ? **PegarTrilhasDoUsuario_DeveRetornarListaVaziaQuandoNaoHouver**: Verifica se retorna lista vazia quando usuário não possui trilhas

### 3. **ConteudoTrilhaUseCaseTests** (`Tests/UnitTests/Application/ConteudoTrilhaUseCaseTests.cs`)
Testa a use case `ConteudoTrilhaUseCase` com as seguintes verificações:

- ? **PegarTodasOsConteudosTrilha_DeveRetornarLista**: Verifica se retorna lista de conteúdos
- ? **PegarConteudoTrilha_DeveRetornarConteudoQuandoExistir**: Verifica se retorna conteúdo específico
- ? **PegarConteudoTrilha_DeveRetornarNuloQuandoNaoExistir**: Verifica se retorna nulo quando não existe
- ? **PegarTodasOsConteudosTrilha_DeveRetornarListaVaziaQuandoNaoHouver**: Verifica se retorna lista vazia

### 4. **ConteudoTrilhaUsuarioUseCaseTests** (`Tests/UnitTests/Application/ConteudoTrilhaUsuarioUseCaseTests.cs`)
Testa a use case `ConteudoTrilhaUsuarioUseCase` com as seguintes verificações:

- ? **PegarTodasOsConteudosTrilhaUsuario_DeveRetornarLista**: Verifica se retorna lista de conteúdos do usuário
- ? **PegarConteudoTrilhaUsuario_DeveRetornarConteudoQuandoExistir**: Verifica se retorna conteúdo específico do usuário
- ? **ConcluirConteudoTrilhaUsuario_DeveConcluir**: Verifica se conclui conteúdo corretamente
- ? **ConcluirConteudoTrilhaUsuario_DeveConcluirTrilhaQuandoCompleta**: Verifica se conclui trilha quando todos conteúdos estão completos
- ? **ConcluirConteudoTrilhaUsuario_NaoDeveConcluirTrilhaQuandoHouverIncompletos**: Verifica que não conclui trilha se há conteúdos incompletos
- ? **PegarConteudoTrilhaUsuario_DeveRetornarNuloQuandoNaoExistir**: Verifica se retorna nulo quando não existe

## Padrão de Teste Utilizado

Cada teste segue a estrutura AAA (Arrange-Act-Assert):

```csharp
[Fact(DisplayName = "Descrição clara do teste")]
public async Task NomeDeTeste()
{
    // Arrange - Prepara os dados e configura os mocks
    var dados = ...;
    _repositoryMock.Setup(...);

    // Act - Executa a ação a ser testada
    var result = await _useCase.Metodo(...);

    // Assert - Verifica os resultados
    Assert.NotNull(result);
    _repositoryMock.Verify(...);
}
```

## Verificações de Mocks

Todos os testes verificam:
- ? Que o método retorna os dados esperados
- ? Que o repositório foi chamado corretamente
- ? Que as chamadas foram feitas com os parâmetros corretos
- ? Comportamentos esperados em casos de sucesso e falha

## Como Executar os Testes

```bash
# Executar todos os testes
dotnet test

# Executar testes de um arquivo específico
dotnet test Tests/UnitTests/Application/TrilhaUseCaseTests.cs

# Executar testes com verbosidade
dotnet test --verbosity normal
```

## Dependências Utilizadas

- **xUnit**: Framework de testes
- **Moq**: Biblioteca para mocking de objetos
- **Microsoft.Extensions.Logging.Abstractions**: Para logging

## Estrutura de Diretórios

```
Tests/
??? UnitTests/
?   ??? Application/
?   ?   ??? TrilhaUseCaseTests.cs
?   ?   ??? TrilhaUsuarioUseCaseTests.cs
?   ?   ??? ConteudoTrilhaUseCaseTests.cs
?   ?   ??? ConteudoTrilhaUsuarioUseCaseTests.cs
?   ??? MockLogger.cs
??? Tests.csproj
```

## Total de Testes: 20+ testes unitários

Todos os testes foram compilados com sucesso ?
