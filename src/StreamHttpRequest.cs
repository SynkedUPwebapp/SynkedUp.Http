using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EL.Http
{
    public class StreamHttpRequest: IHttpRequest
    {
        public StreamHttpRequest()
        {
            Headers = new HttpHeaders();
        }

        public HttpMethod Method { get; set; }
        public string Url { get; set; }
        public HttpHeaders Headers { get; set; }
        public Stream Body { get; set; }

        public bool HasContent() => Body != null;

        public HttpContent GetContent() => new StreamContent(Body);
    }
}
