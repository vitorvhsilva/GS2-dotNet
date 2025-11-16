namespace API.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_ENDERECO_USUARIO")]
public class EnderecoUsuario
{
    [Key]
    [ForeignKey(nameof(Usuario))]
    [Column("ID_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdUsuario { get; set; } = null!;

    [Column("CEP_ENDERECO", TypeName = "VARCHAR2(20)")]
    public string CepEndereco { get; set; } = null!;

    [Column("LOGRADOURO_ENDERECO", TypeName = "VARCHAR2(200)")]
    public string? LogradouroEndereco { get; set; }

    [Column("ESTADO_ENDERECO", TypeName = "VARCHAR2(200)")]
    public string? EstadoEndereco { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
