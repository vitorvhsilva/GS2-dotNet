using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.AppData;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<EnderecoUsuario> EnderecosUsuarios { get; set; }
    public DbSet<FormularioProfissaoUsuario> FormulariosProfissaoUsuario { get; set; }
    public DbSet<Trilha> Trilhas { get; set; }
    public DbSet<TrilhaUsuario> TrilhasUsuarios { get; set; }
    public DbSet<ConteudoTrilha> ConteudosTrilha { get; set; }
    public DbSet<ConteudoTrilhaUsuario> ConteudosTrilhaUsuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("TB_USUARIO");

            entity.HasKey(e => e.IdUsuario)
                  .HasName("TB_USUARIO_ID_USUARIO_PK");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.NomeUsuario).HasColumnName("NOME_USUARIO");
            entity.Property(e => e.EmailUsuario).HasColumnName("EMAIL_USUARIO");
            entity.Property(e => e.SenhaUsuario).HasColumnName("SENHA_USUARIO");
            entity.Property(e => e.DataNascimentoUsuario).HasColumnName("DATA_NASCIMENTO_USUARIO");
        });

        modelBuilder.Entity<EnderecoUsuario>(entity =>
        {
            entity.ToTable("TB_ENDERECO_USUARIO");

            entity.HasKey(e => e.IdUsuario)
                  .HasName("TB_ENDERECO_USUARIO_ID_USUARIO_PK");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.CepEndereco).HasColumnName("CEP_ENDERECO");
            entity.Property(e => e.LogradouroEndereco).HasColumnName("LOGRADOURO_ENDERECO");
            entity.Property(e => e.EstadoEndereco).HasColumnName("ESTADO_ENDERECO");
        });

        modelBuilder.Entity<FormularioProfissaoUsuario>(entity =>
        {
            entity.ToTable("TB_FORMULARIO_PROFISSAO_USUARIO");

            entity.HasKey(e => e.IdUsuario)
                  .HasName("TB_FORMULARIO_PROFISSAO_USUARIO_ID_USUARIO_PK");

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.RespostaPergunta1).HasColumnName("RESPOSTA_PERGUNTA_1");
            entity.Property(e => e.RespostaPergunta2).HasColumnName("RESPOSTA_PERGUNTA_2");
            entity.Property(e => e.RespostaPergunta3).HasColumnName("RESPOSTA_PERGUNTA_3");
            entity.Property(e => e.RespostaPergunta4).HasColumnName("RESPOSTA_PERGUNTA_4");
            entity.Property(e => e.RespostaPergunta5).HasColumnName("RESPOSTA_PERGUNTA_5");
            entity.Property(e => e.RespostaPergunta6).HasColumnName("RESPOSTA_PERGUNTA_6");
            entity.Property(e => e.RespostaPergunta7).HasColumnName("RESPOSTA_PERGUNTA_7");
            entity.Property(e => e.RespostaPergunta8).HasColumnName("RESPOSTA_PERGUNTA_8");
            entity.Property(e => e.RespostaPergunta9).HasColumnName("RESPOSTA_PERGUNTA_9");
            entity.Property(e => e.RespostaPergunta10).HasColumnName("RESPOSTA_PERGUNTA_10");
            entity.Property(e => e.ProfissaoRecomendada).HasColumnName("PROFISSAO_RECOMENDADA");
        });

        modelBuilder.Entity<Trilha>(entity =>
        {
            entity.ToTable("TB_TRILHA");

            entity.HasKey(e => e.IdTrilha)
                  .HasName("TB_TRILHA_ID_TRILHA_PK");

            entity.Property(e => e.IdTrilha).HasColumnName("ID_TRILHA");
            entity.Property(e => e.NomeTrilha).HasColumnName("NOME_TRILHA");
            entity.Property(e => e.QuantidadeConteudoTrilha).HasColumnName("QUANTIDADE_CONTEUDO_TRILHA");
        });

        modelBuilder.Entity<ConteudoTrilha>(entity =>
        {
            entity.ToTable("TB_CONTEUDO_TRILHA");

            entity.HasKey(e => e.IdConteudoTrilha)
                  .HasName("TB_CONTEUDO_TRILHA_ID_CONTEUDO_TRILHA_PK");

            entity.Property(e => e.IdConteudoTrilha).HasColumnName("ID_CONTEUDO_TRILHA");
            entity.Property(e => e.NomeConteudoTrilha).HasColumnName("NOME_CONTEUDO_TRILHA");
            entity.Property(e => e.TipoConteudoTrilha).HasColumnName("TIPO_CONTEUDO_TRILHA");
            entity.Property(e => e.TextoConteudoTrilha).HasColumnName("TEXTO_CONTEUDO_TRILHA");
            entity.Property(e => e.IdTrilha).HasColumnName("ID_TRILHA");
        });

        modelBuilder.Entity<ConteudoTrilhaUsuario>(entity =>
        {
            entity.ToTable("TB_CONTEUDO_TRILHA_USUARIO");

            entity.HasKey(e => e.IdConteudoTrilhaUsuario)
                  .HasName("TB_CONTEUDO_TRILHA_USUARIO_ID_CONTEUDO_TRILHA_USUARIO_PK");

            entity.Property(e => e.IdConteudoTrilhaUsuario).HasColumnName("ID_CONTEUDO_TRILHA_USUARIO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.IdConteudoTrilha).HasColumnName("ID_CONTEUDO_TRILHA");
            entity.Property(e => e.ConteudoTrilhaConcluidaUsuario).HasColumnName("CONTEUDO_TRILHA_CONCLUIDA_USUARIO");
        });

        modelBuilder.Entity<TrilhaUsuario>(entity =>
        {
            entity.ToTable("TB_TRILHA_USUARIO");

            entity.HasKey(e => e.IdTrilhaUsuario)
                  .HasName("TB_TRILHA_USUARIO_ID_TRILHA_USUARIO_PK");

            entity.Property(e => e.IdTrilhaUsuario).HasColumnName("ID_TRILHA_USUARIO");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.IdTrilha).HasColumnName("ID_TRILHA");
            entity.Property(e => e.TrilhaConcluidaUsuario).HasColumnName("TRILHA_CONCLUIDA_USUARIO");
        });
    }
}
