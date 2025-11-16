namespace API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TB_USUARIO")]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO", TypeName = "VARCHAR2(36)")]
    public string IdUsuario { get; set; } = null!;

    [Column("NOME_USUARIO", TypeName = "VARCHAR2(255)")]
    public string NomeUsuario { get; set; } = null!;

    [Column("EMAIL_USUARIO", TypeName = "VARCHAR2(255)")]
    public string EmailUsuario { get; set; } = null!;

    [Column("SENHA_USUARIO", TypeName = "VARCHAR2(255)")]
    public string SenhaUsuario { get; set; } = null!;

    [Column("DATA_NASCIMENTO_USUARIO", TypeName = "TIMESTAMP")]
    public DateTime DataNascimentoUsuario { get; set; }

    
    public EnderecoUsuario? EnderecoUsuario { get; set; }
    public FormularioProfissaoUsuario? FormularioProfissaoUsuario { get; set; }
    public ICollection<ConteudoTrilhaUsuario> ConteudosTrilhaUsuario { get; set; } = new List<ConteudoTrilhaUsuario>();
    public ICollection<TrilhaUsuario> TrilhasUsuario { get; set; } = new List<TrilhaUsuario>();
}
