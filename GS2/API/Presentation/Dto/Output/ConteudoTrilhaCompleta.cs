namespace API.Presentation.Dto.Output
{
    public record ConteudoTrilhaCompleta(
        string IdConteudoTrilha,
        string NomeConteudoTrilha,
        string TipoConteudoTrilha, 
        string TextoConteudoTrilha, 
        bool ConteudoTrilhaConcluida
    );
}
