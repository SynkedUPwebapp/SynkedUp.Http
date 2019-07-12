namespace EL.Http
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            Headers = new HttpHeaders();
        }

        public HttpMethod Method { get; set; }
        public string Url { get; set; }
        public HttpHeaders Headers { get; set; }
        public string Body { get; set; }
    }
}