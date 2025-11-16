namespace API.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_TRILHA_USUARIO")]
public class TrilhaUsuario
{
    [Key]
    [Column("ID_TRILHA_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdTrilhaUsuario { get; set; } = null!;

    [ForeignKey(nameof(Usuario))]
    [Column("ID_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdUsuario { get; set; } = null!;

    [ForeignKey(nameof(Trilha))]
    [Column("ID_TRILHA", TypeName = "VARCHAR2(36)")]
    public string IdTrilha { get; set; } = null!;

    [Column("TRILHA_CONCLUIDA_USUARIO", TypeName = "CHAR(1)")]
    public string? TrilhaConcluidaUsuario { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public Trilha Trilha { get; set; } = null!;
}
