namespace API.Presentation.Dto.Output
{
    public class PaginacaoResponse<T>
    {
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalItens { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginacaoResponse(
            IEnumerable<T> data,
            int paginaAtual,
            int tamanhoPagina,
            int totalItens)
        {
            Data = data;
            PaginaAtual = paginaAtual;
            TamanhoPagina = tamanhoPagina;
            TotalItens = totalItens;
            TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
        }

        public static PaginacaoResponse<T> Criar(
            IEnumerable<T> source,
            int pagina,
            int tamanhoPagina)
        {
            if (pagina < 1) pagina = 1;
            if (tamanhoPagina < 1) tamanhoPagina = 5;

            var totalItens = source.Count();

            var itensPaginados = source
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            return new PaginacaoResponse<T>(
                itensPaginados,
                pagina,
                tamanhoPagina,
                totalItens
            );
        }
    }
}
