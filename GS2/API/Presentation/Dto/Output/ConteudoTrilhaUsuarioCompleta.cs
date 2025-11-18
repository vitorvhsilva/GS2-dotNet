namespace API.Presentation.Dto.Output
{
    public record ConteudoTrilhaUsuarioCompleta(
        string IdConteudoTrilha,
        string NomeConteudoTrilha,
        string TipoConteudoTrilha, 
        string TextoConteudoTrilha,
        bool ConteudoTrilhaConcluida
    );
}
