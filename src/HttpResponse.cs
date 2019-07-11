namespace EL.Http
{
    public class HttpResponse
    {
        public int StatusCode { get; private set; }
        public HttpHeaders Headers { get; private set; }
        public string Body { get; private set; }

        public HttpResponse(int statusCode, HttpHeaders headers, string body)
        {
            StatusCode = statusCode;
            Headers = headers;
            Body = body;
        }
    }
}