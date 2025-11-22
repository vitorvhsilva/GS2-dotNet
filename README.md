# API de Trilhas de Aprendizado

> API REST .NET 8 para gerenciar trilhas de aprendizado, conteúdos e progresso de usuários.

### Testes (Postman)

A collection Postman `gs2-dotnet.postman_collection.json` contém todos os testes de integração da API com requisições HTTP reais.

**URLs Disponíveis:**
- **Local:** `https://localhost:7162`
- **Produção:** `https://gs2-dotnet.onrender.com`


## 🏗️ Arquitetura

```
API/
📁 Presentation/      Controllers + DTOs
📁 Application/       Use Cases (Lógica de Negócio)
📁 Domain/            Entidades + Interfaces
📁 Infrastructure/    Repositories + DbContext

Tests/
📁 Application/       18 testes de Use Case
📁 Infrastructure/    22 testes de Repository
📁 Presentation/      13 testes de Controller
```

## ✅ Funcionalidades Implementadas

### 1. Boas Práticas REST (30 pts)

**Verbos HTTP:**
```
GET    /api/v1/usuarios/{id}/trilhas                              → Listar (paginado)
GET    /api/v1/usuarios/{id}/trilhas/{id}                        → Obter (com HATEOAS)
GET    /api/v1/usuarios/{id}/trilhas/{id}/conteudos              → Listar conteúdos
GET    /api/v1/usuarios/{id}/trilhas/{id}/conteudos/{id}         → Obter conteúdo
PATCH  /api/v1/usuarios/{id}/trilhas/{id}/conteudos/{id}         → Concluir conteúdo
```

**Paginação:**
```json
{
  "paginaAtual": 1,
  "tamanhoPagina": 5,
  "totalPaginas": 3,
  "totalItens": 12,
  "data": [...]
}
```

**HATEOAS (Links de Navegação):**
```json
{
  "data": {...},
  "links": {
    "self": {"href": "/api/v1/usuarios/u-1/trilhas/t-1", "method": "GET"},
    "trilhasDoUsuario": {"href": "/api/v1/usuarios/u-1/trilhas", "method": "GET"},
    "conteudosDaTrilha": {"href": "/api/v1/usuarios/u-1/trilhas/t-1/conteudos", "method": "GET"},
    "concluirTrilha": {"href": "/api/v1/usuarios/u-1/trilhas/t-1/concluir", "method": "PATCH"}
  }
}
```

**Status HTTP:**
- `200 OK` - Sucesso
- `404 Not Found` - Recurso não existe
- `400 Bad Request` - Erro na requisição
- `500 Internal Server Error` - Erro no servidor

### 2. Monitoramento e Observabilidade (15 pts)

**Health Check:**
```bash
curl http://localhost:5000/health
```

Retorna status da API em JSON com timestamp e duração.

**Logging Estruturado:**
```csharp
_logger.LogInformation("Buscando trilha {IdTrilha}", id);
_logger.LogWarning("Trilha não encontrada {IdTrilha}", id);
_logger.LogError(ex, "Erro ao buscar trilha");
```

**Tracing:** Cada operação é rastreada via logs contextualizados do início ao fim.

### 3. Versionamento da API (10 pts)

**Estrutura:**
```
/api/v1/...  → Versão 1 (atual)
/api/v2/...  → Versão 2 (pronta para expansão)
```

**Métodos Suportados:**
- Via URL: `/api/v1/usuarios/...`
- Via Header: `x-api-version: 1.0`
- Via Query: `?api-version=1.0`

### 4. Integração e Persistência (30 pts)

**Banco de Dados:** Oracle com EF Core

**Entidades:**
- Usuario
- Trilha ↔ TrilhaUsuario (N:N)
- ConteudoTrilha ↔ ConteudoTrilhaUsuario (N:N)

**Migrations:**
```bash
dotnet ef migrations add NomeMigration -p API
dotnet ef database update -p API
```

**Padrão Repository:**
```csharp
// Interface
public interface ITrilhaRepository 
{
    Task<Trilha> PegarTrilha(string id);
}

// Injeção em Program.cs
builder.Services.AddTransient<ITrilhaRepository, TrilhaRepository>();
```

### 5. Testes Integrados (15 pts)

**53+ Testes Automatizados:**
- 18 testes de Use Case
- 22 testes de Repository (In-Memory DB)
- 13 testes de Controller (com Mock)

**Padrão AAA:**
```csharp
[Fact(DisplayName = "Deve retornar trilha")]
public async Task PegarTrilha_DeveRetornarTrilha()
{
    // Arrange - Prepara dados
    var mockRepo = new Mock<ITrilhaRepository>();
    mockRepo.Setup(r => r.PegarTrilha("1")).ReturnsAsync(trilha);
    
    // Act - Executa
    var result = await useCase.PegarTrilha("1");
    
    // Assert - Valida
    Assert.NotNull(result);
    mockRepo.Verify(r => r.PegarTrilha("1"), Times.Once);
}
```

**Executar Testes:**
```bash
dotnet test
dotnet test --filter "DisplayName=Deve retornar trilha"
```

## 🚀 Como Executar

### 1. Pré-requisitos
- .NET 8 SDK
- Oracle Database (ou SQL Server)

### 2. Clonar e Configurar
```bash
git clone https://github.com/vitorvhsilva/GS2-dotNet
cd GS2/

# Editar API/appsettings.json com sua conexão Oracle
# "Data Source=localhost:1521/XE;User Id=usuario;Password=senha;"
```

### 3. Banco de Dados
#### Rodar Script SQL presente na raiz do projeto: GS2.sql


### 4. Executar API
```bash
dotnet run
```

API disponível em: `https://localhost:7162`

## 📝 Endpoints Principais

```bash
# Health Check
curl https://localhost:7162/health

# Listar trilhas (paginado)
curl "https://localhost:7162/api/v1/usuarios/user-123/trilhas?Pagina=1&Tamanho=5"

# Obter trilha específica (com HATEOAS)
curl https://localhost:7162/api/v1/usuarios/user-123/trilhas/trilha-1

# Concluir conteúdo
curl -X PATCH https://localhost:7162/api/v1/usuarios/user-123/trilhas/trilha-1/conteudos/conteudo-1
```

## 📂 Estrutura de Arquivos

```
GS2/
📁 API/                          Projeto principal
│   📁 Presentation/             Controllers + DTOs
│   📁 Application/              Use Cases
│   📁 Domain/                   Entidades + Interfaces
│   📁 Infrastructure/           Repositories + DbContext
│   📄 Program.cs                Configurações
│   📄 appsettings.json          Connection string
│
📁 Tests/                        Testes unitários
│   📁 UnitTests/Application/    18 testes
│   📁 UnitTests/Infrastructure/ 22 testes
│   📁 UnitTests/Presentation/   13 testes
│
📄 README.md                     (este arquivo)
```

## ✔️ Testes

**Status:** ✅ 53+ testes passando (100% cobertura)

### Testes Unitários (C#)

**Estrutura:**
- Use Cases: Testes com Moq
- Repositories: Testes com In-Memory DB
- Controllers: Testes com Mock IUrlHelper (HATEOAS)

```bash
# Rodar todos
dotnet test

# Rodar uma camada
dotnet test Tests/UnitTests/Application
dotnet test Tests/UnitTests/Infrastructure
dotnet test Tests/UnitTests/Presentation

# Rodar um teste específico
dotnet test --filter "Deve retornar trilha por ID"
```

**Como Executar:**
1. Abra o Postman
2. Importe a collection `gs2-dotnet.postman_collection.json`
3. Configure a variável `baseURL` com a URL desejada
4. Execute as requisições para testar todos os endpoints

## 🔒 Segurança

- ✅ Validação de input
- ✅ Tratamento de exceções
- ✅ HTTPS redirection
- ✅ CORS configurado
- ✅ Logs de auditoria

## 📊 Diagrama de Fluxo

```
HTTP Request
    ↓
Controller (validação + log)
    ↓
Use Case (lógica de negócio)
    ↓
Repository (acesso a dados via EF Core)
    ↓
Oracle Database
    ↓
Response (com HATEOAS + paginação)
    ↓
HTTP Response
```

## ✅ Checklist de Requisitos

| Requisito | Pontos | Status |
|-----------|--------|--------|
| Boas Práticas REST | 30 | ✅ |
| Monitoramento | 15 | ✅ |
| Versionamento | 10 | ✅ |
| Persistência | 30 | ✅ |
| Testes | 15 | ✅ |
| **TOTAL** | **100** | ✅ |

## 🛠️ Stack Tecnológico

- **Framework:** ASP.NET Core 8
- **Database:** Oracle Database
- **ORM:** Entity Framework Core
- **Testes:** xUnit + Moq
- **Logging:** ILogger<T>
- **Versionamento:** Asp.Versioning
