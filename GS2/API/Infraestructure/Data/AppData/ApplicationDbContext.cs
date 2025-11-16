using API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Infraestructure.Data.AppData;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}

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

            entity.HasKey(e => e.IdUsuario);

            entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");
            entity.Property(e => e.NomeUsuario).HasColumnName("NOME_USUARIO");
            entity.Property(e => e.EmailUsuario).HasColumnName("EMAIL_USUARIO");
            entity.Property(e => e.SenhaUsuario).HasColumnName("SENHA_USUARIO");
            entity.Property(e => e.DataNascimentoUsuario).HasColumnName("DATA_NASCIMENTO_USUARIO");

            entity.HasOne(e => e.EnderecoUsuario)
                .WithOne(e => e.Usuario)
                .HasForeignKey<EnderecoUsuario>(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FormularioProfissaoUsuario)
                .WithOne(e => e.Usuario)
                .HasForeignKey<FormularioProfissaoUsuario>(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.ConteudosTrilhaUsuario)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.TrilhasUsuario)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EnderecoUsuario>(entity =>
        {
            entity.ToTable("TB_ENDERECO_USUARIO");
            entity.HasKey(e => e.IdUsuario);
        });

        modelBuilder.Entity<FormularioProfissaoUsuario>(entity =>
        {
            entity.ToTable("TB_FORMULARIO_PROFISSAO_USUARIO");
            entity.HasKey(e => e.IdUsuario);
        });

        modelBuilder.Entity<Trilha>(entity =>
        {
            entity.ToTable("TB_TRILHA");
            entity.HasKey(e => e.IdTrilha);

            entity.HasMany(e => e.ConteudosTrilha)
                .WithOne(e => e.Trilha)
                .HasForeignKey(e => e.IdTrilha)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.TrilhasUsuario)
                .WithOne(e => e.Trilha)
                .HasForeignKey(e => e.IdTrilha)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConteudoTrilha>(entity =>
        {
            entity.ToTable("TB_CONTEUDO_TRILHA");
            entity.HasKey(e => e.IdConteudoTrilha);

            entity.HasMany(e => e.ConteudosUsuarios)
                .WithOne(e => e.ConteudoTrilha)
                .HasForeignKey(e => e.IdConteudoTrilha)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConteudoTrilhaUsuario>(entity =>
        {
            entity.ToTable("TB_CONTEUDO_TRILHA_USUARIO");
            entity.HasKey(e => e.IdConteudoTrilhaUsuario);
        });

        modelBuilder.Entity<TrilhaUsuario>(entity =>
        {
            entity.ToTable("TB_TRILHA_USUARIO");
            entity.HasKey(e => e.IdTrilhaUsuario);
        });
    }
}