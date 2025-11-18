namespace API.Presentation.Dto.Output
{
    public class HateoasLink
    {
        public string Href { get; }
        public string Method { get; }

        public HateoasLink(string href, string method)
        {
            Href = href;
            Method = method;
        }
    }
}
