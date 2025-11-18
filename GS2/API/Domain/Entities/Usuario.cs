using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_USUARIO")]
    public class Usuario
    {
        [Key]
        public string IdUsuario { get; set; } = null!;

        public string NomeUsuario { get; set; } = null!;

        public string EmailUsuario { get; set; } = null!;

        public string SenhaUsuario { get; set; } = null!;

        public DateTime DataNascimentoUsuario { get; set; }

        public EnderecoUsuario? EnderecoUsuario { get; set; }
        public FormularioProfissaoUsuario? FormularioProfissaoUsuario { get; set; }

        public ICollection<TrilhaUsuario> TrilhasUsuario { get; set; } = new List<TrilhaUsuario>();
        public ICollection<ConteudoTrilhaUsuario> ConteudosTrilhaUsuario { get; set; } = new List<ConteudoTrilhaUsuario>();
    }
}
