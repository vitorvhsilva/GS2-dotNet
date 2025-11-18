using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_TRILHA")]
    public class Trilha
    {
        [Key]
        public string IdTrilha { get; set; } = null!;

        public string NomeTrilha { get; set; } = null!;

        public int QuantidadeConteudoTrilha { get; set; }

        public ICollection<ConteudoTrilha> ConteudosTrilha { get; set; } = new List<ConteudoTrilha>();
        public ICollection<TrilhaUsuario> TrilhasUsuario { get; set; } = new List<TrilhaUsuario>();
    }
}
