namespace API.Presentation.Dto.Output
{
    public record TrilhaUsuarioCompleta(
        string IdTrilha,
        string NomeTrilha,
        int QuantidadeConteudosTrilha,
        bool TrilhaCompletada
    );
}
