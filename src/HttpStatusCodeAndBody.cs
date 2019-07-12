using System.Net;

namespace EL.Http
{
    public class HttpStatusCodeAndBody
    {
        public string Body { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsResponseSuccessful()
        {
            return (int) StatusCode >= 200 && (int) StatusCode < 300;
        }
    }
}