namespace API.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_TRILHA")]
public class Trilha
{
    [Key]
    [Column("ID_TRILHA", TypeName = "VARCHAR2(36)")]
    public string IdTrilha { get; set; } = null!;

    [Column("NOME_TRILHA", TypeName = "VARCHAR2(200)")]
    public string NomeTrilha { get; set; } = null!;

    [Column("QUANTIDADE_CONTEUDO_TRILHA", TypeName = "VARCHAR2(200)")]
    public string QuantidadeConteudoTrilha { get; set; } = null!;

    
    public ICollection<ConteudoTrilha> ConteudosTrilha { get; set; } = new List<ConteudoTrilha>();
    public ICollection<TrilhaUsuario> TrilhasUsuario { get; set; } = new List<TrilhaUsuario>();
}