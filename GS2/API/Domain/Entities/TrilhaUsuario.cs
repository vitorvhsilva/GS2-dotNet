using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_TRILHA_USUARIO")]
    public class TrilhaUsuario
    {
        [Key]
        public string IdTrilhaUsuario { get; set; } = null!;

        [ForeignKey(nameof(Usuario))]
        public string IdUsuario { get; set; } = null!;

        [ForeignKey(nameof(Trilha))]
        public string IdTrilha { get; set; } = null!;

        public string TrilhaConcluidaUsuario { get; set; } = "N";

        public Usuario Usuario { get; set; } = null!;
        public Trilha Trilha { get; set; } = null!;
    }
}
