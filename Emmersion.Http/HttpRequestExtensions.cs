using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Emmersion.Http
{
    public static class HttpRequestExtensions
    {
        public static void AddJsonBody(this HttpRequest request, object bodyObject)
        {
            request.Body = CamelCaseJsonSerializer.Serialize(bodyObject);
            request.Headers.Add("Content-Type", "application/json");
        }

        public static void AddBasicAuthentication(this IHttpRequest request, string username, string password)
        {
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            request.Headers.Add("Authorization", $"Basic {encodedCredentials}");
        }
        
        public static async Task AddFormUrlEncodedBody(this HttpRequest request, Dictionary<string, string> bodyObject)
        {
            request.Body = await new FormUrlEncodedContent(bodyObject).ReadAsStringAsync();
            request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        }
    }
}
