using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_CONTEUDO_TRILHA")]
    public class ConteudoTrilha
    {
        [Key]
        public string IdConteudoTrilha { get; set; } = null!;

        public string NomeConteudoTrilha { get; set; } = null!;

        public string TipoConteudoTrilha { get; set; } = null!;
        public string TextoConteudoTrilha { get; set; } = null!;

        [ForeignKey(nameof(Trilha))]
        public string IdTrilha { get; set; } = null!;

        public Trilha Trilha { get; set; } = null!;
        public ICollection<ConteudoTrilhaUsuario> ConteudosUsuarios { get; set; } = new List<ConteudoTrilhaUsuario>();
    }
}
