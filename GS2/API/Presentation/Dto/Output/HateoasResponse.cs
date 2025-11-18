namespace API.Presentation.Dto.Output
{
    public class HateoasResponse<T>
    {
        public T Data { get; set; }
        public Dictionary<string, HateoasLink> Links { get; set; }

        public HateoasResponse(T data)
        {
            Data = data;
            Links = new Dictionary<string, HateoasLink>();
        }

        public void AddLink(string rel, string href, string method)
        {
            Links[rel] = new HateoasLink(href, method);
        }
    }
}
