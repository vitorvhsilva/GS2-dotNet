using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    [Table("TB_ENDERECO_USUARIO")]
    public class EnderecoUsuario
    {
        [Key]
        [ForeignKey(nameof(Usuario))]
        public string IdUsuario { get; set; } = null!;

        public string CepEndereco { get; set; } = null!;

        public string? LogradouroEndereco { get; set; }

        public string? EstadoEndereco { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}
