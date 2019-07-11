using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EL.Http
{
    public class HttpRequest
    {
        public static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public HttpMethod Method { get; set; }
        public string Url { get; set; }
        public HttpHeaders Headers { get; set; }
        public string Body { get; set; }

        public HttpRequest()
        {
            Headers = new HttpHeaders();
        }

        public void AddJsonBody(object bodyObject)
        {
            Body = JsonConvert.SerializeObject(bodyObject, serializationSettings);
            Headers.Add("Content-Type", "application/json");
        }
    }
}