namespace API.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_FORMULARIO_PROFISSAO_USUARIO")]
public class FormularioProfissaoUsuario
{
    [Key]
    [ForeignKey(nameof(Usuario))]
    [Column("ID_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdUsuario { get; set; } = null!;

    [Column("RESPOSTA_PERGUNTA_1", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta1 { get; set; }

    [Column("RESPOSTA_PERGUNTA_2", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta2 { get; set; }

    [Column("RESPOSTA_PERGUNTA_3", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta3 { get; set; }

    [Column("RESPOSTA_PERGUNTA_4", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta4 { get; set; }

    [Column("RESPOSTA_PERGUNTA_5", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta5 { get; set; }

    [Column("RESPOSTA_PERGUNTA_6", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta6 { get; set; }

    [Column("RESPOSTA_PERGUNTA_7", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta7 { get; set; }

    [Column("RESPOSTA_PERGUNTA_8", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta8 { get; set; }

    [Column("RESPOSTA_PERGUNTA_9", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta9 { get; set; }

    [Column("RESPOSTA_PERGUNTA_10", TypeName = "VARCHAR2(1000)")]
    public string? RespostaPergunta10 { get; set; }

    [Column("PROFISSAO_RECOMENDADA", TypeName = "VARCHAR2(100)")]
    public string? ProfissaoRecomendada { get; set; }

    public Usuario Usuario { get; set; } = null!;
}
