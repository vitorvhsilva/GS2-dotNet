namespace API.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_CONTEUDO_TRILHA")]
public class ConteudoTrilha
{
    [Key]
    [Column("ID_CONTEUDO_TRILHA", TypeName = "VARCHAR2(36)")]
    public string IdConteudoTrilha { get; set; } = null!;

    [Column("NOME_CONTEUDO_TRILHA", TypeName = "VARCHAR2(200)")]
    public string NomeConteudoTrilha { get; set; } = null!;

    [Column("TIPO_CONTEUDO_TRILHA", TypeName = "VARCHAR2(20)")]
    public string TipoConteudoTrilha { get; set; } = null!;

    [ForeignKey(nameof(Trilha))]
    [Column("ID_TRILHA", TypeName = "VARCHAR2(36)")]
    public string IdTrilha { get; set; } = null!;

    public Trilha Trilha { get; set; } = null!;
    public ICollection<ConteudoTrilhaUsuario> ConteudosUsuarios { get; set; } = new List<ConteudoTrilhaUsuario>();
}