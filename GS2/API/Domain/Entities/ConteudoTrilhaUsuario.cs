namespace API.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_CONTEUDO_TRILHA_USUARIO")]
public class ConteudoTrilhaUsuario
{
    [Key]
    [Column("ID_CONTEUDO_TRILHA_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdConteudoTrilhaUsuario { get; set; } = null!;

    [Column("CONTEUDO_TRILHA_CONCLUIDA_USUARIO", TypeName = "CHAR(1)")]
    public string? ConteudoTrilhaConcluidaUsuario { get; set; }

    [ForeignKey(nameof(Usuario))]
    [Column("ID_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdUsuario { get; set; } = null!;

    [ForeignKey(nameof(ConteudoTrilha))]
    [Column("ID_CONTEUDO_TRILHA", TypeName = "VARCHAR2(36)")]
    public string IdConteudoTrilha { get; set; } = null!;

    public Usuario Usuario { get; set; } = null!;
    public ConteudoTrilha ConteudoTrilha { get; set; } = null!;
}
