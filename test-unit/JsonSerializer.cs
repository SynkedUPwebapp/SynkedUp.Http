using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EL.Http.UnitTests
{
    public class JsonSerializer : IRequestSerializer
    {
        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

        public string SerializeBody(object body)
        {
            return JsonConvert.SerializeObject(body, serializationSettings);
        }
    }
}