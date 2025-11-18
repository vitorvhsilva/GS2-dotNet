using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_CONTEUDO_TRILHA_USUARIO")]
    public class ConteudoTrilhaUsuario
    {
        [Key]
        public string IdConteudoTrilhaUsuario { get; set; } = null!;

        public string? ConteudoTrilhaConcluidaUsuario { get; set; }

        [ForeignKey(nameof(Usuario))]
        public string IdUsuario { get; set; } = null!;

        [ForeignKey(nameof(ConteudoTrilha))]
        public string IdConteudoTrilha { get; set; } = null!;

        public Usuario Usuario { get; set; } = null!;
        public ConteudoTrilha ConteudoTrilha { get; set; } = null!;
    }
}
