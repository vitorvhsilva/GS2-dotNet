using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_FORMULARIO_PROFISSAO_USUARIO")]
    public class FormularioProfissaoUsuario
    {
        [Key]
        [ForeignKey(nameof(Usuario))]
        public string IdUsuario { get; set; } = null!;

        public string? RespostaPergunta1 { get; set; }

        public string? RespostaPergunta2 { get; set; }

        public string? RespostaPergunta3 { get; set; }

        public string? RespostaPergunta4 { get; set; }

        public string? RespostaPergunta5 { get; set; }

        public string? RespostaPergunta6 { get; set; }

        public string? RespostaPergunta7 { get; set; }

        public string? RespostaPergunta8 { get; set; }

        public string? RespostaPergunta9 { get; set; }

        public string? RespostaPergunta10 { get; set; }

        public string? ProfissaoRecomendada { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}
