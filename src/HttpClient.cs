using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EL.Http
{
    public interface IHttpClient
    {
        HttpResponse Execute(HttpRequest request);
        HttpResponse Execute(HttpRequest request, int timeoutMilliseconds);
        Task<HttpResponse> ExecuteAsync(HttpRequest request);
    }

    public class HttpClient : IHttpClient, IDisposable
    {
        private readonly System.Net.Http.HttpClient client;

        public HttpClient() : this(null)
        {
        }

        public HttpClient(HttpClientOptions options)
        {
            if (options == null) options = new HttpClientOptions();
            var handler = new HttpClientHandler
            {
                UseCookies = false,
                AllowAutoRedirect = options.AllowAutoRedirect
            };
            client = new System.Net.Http.HttpClient(handler);
            if (options.DefaultTimeoutMilliseconds > 0)
            {
                client.Timeout = TimeSpan.FromMilliseconds(options.DefaultTimeoutMilliseconds);
            }
        }

        public HttpResponse Execute(HttpRequest request)
        {
            return Execute(request, 0);
        }

        public HttpResponse Execute(HttpRequest request, int timeoutMilliseconds)
        {
            var task = ExecuteAsync(request);
            if (timeoutMilliseconds > 0)
            {
                task.Wait(timeoutMilliseconds);
                if (!task.IsCompleted)
                {
                    throw new HttpTimeoutException();
                }
            }

            try
            {
                return task.Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count(e => e is HttpTimeoutException) >= 1)
                {
                    throw new HttpTimeoutException();
                }
                throw;
            }
        }

        public async Task<HttpResponse> ExecuteAsync(HttpRequest request)
        {

            var requestMessage = BuildRequestMessage(request);
            try
            {
                HttpResponseMessage response = await client.SendAsync(requestMessage).ConfigureAwait(false);
                return await BuildResponse(response);
            }
            catch (TaskCanceledException)
            {
                throw new HttpTimeoutException();
            }
        }

        private HttpRequestMessage BuildRequestMessage(HttpRequest request)
        {
            var message = new HttpRequestMessage(GetRequestMethod(request.Method), request.Url);
            foreach (var headerName in request.Headers.GetAllHeaderNames().Where(IsStandardHeaderName))
            {
                message.Headers.Add(headerName, request.Headers.GetAllValues(headerName));
            }

            var acceptHeader = request.Headers.GetAllHeaderNames().FirstOrDefault(x => x.Equals("accept", StringComparison.CurrentCultureIgnoreCase));
            if (acceptHeader != null)
            {
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(request.Headers.GetValue(acceptHeader)));
            }
            if (request.Body != null)
            {
                message.Content = GetRequestContent(request);
            }
            return message;
        }

        private static System.Net.Http.HttpMethod GetRequestMethod(HttpMethod method)
        {
            switch (method)
            {
                case HttpMethod.GET:
                    return System.Net.Http.HttpMethod.Get;
                case HttpMethod.POST:
                    return System.Net.Http.HttpMethod.Post;
                case HttpMethod.PUT:
                    return System.Net.Http.HttpMethod.Put;
                case HttpMethod.DELETE:
                    return System.Net.Http.HttpMethod.Delete;
                default:
                    throw new ArgumentException($"Unsupported HTTP method: '{method}'");
            }
        }

        private static bool IsStandardHeaderName(string name)
        {
            if (name.Equals("accept", StringComparison.CurrentCultureIgnoreCase)) return false;
            if (name.Equals("content-type", StringComparison.CurrentCultureIgnoreCase)) return false;

            return true;
        }

        private static ByteArrayContent GetRequestContent(HttpRequest request)
        {
            var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(request.Body));
            var contentTypeHeader = request.Headers.GetAllHeaderNames().FirstOrDefault(x => x.Equals("content-type", StringComparison.CurrentCultureIgnoreCase));
            if (contentTypeHeader != null)
            {
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(request.Headers.GetValue(contentTypeHeader));
            }

            return byteArrayContent;
        }

        static async Task<HttpResponse> BuildResponse(HttpResponseMessage response)
        {
            if (response == null) throw new Exception("Unable to read web response");

            return new HttpResponse((int)response.StatusCode, GetResponseHeaders(response), await response.Content.ReadAsStringAsync());
        }

        static HttpHeaders GetResponseHeaders(HttpResponseMessage response)
        {
            var headers = new HttpHeaders();

            headers.AddAll(response.Headers);
            headers.AddAll(response.Content.Headers);
            return headers;
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}