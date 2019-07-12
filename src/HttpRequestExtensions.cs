using System;

namespace EL.Http
{
    public static class HttpRequestExtensions
    {
        private static IRequestSerializer requestSerializer;

        public static void InitializeRequestSerializer(IRequestSerializer requestSerializer)
        {
            HttpRequestExtensions.requestSerializer = requestSerializer;
        }

        public static void AddJsonBody(this HttpRequest request, object bodyObject)
        {
            if (requestSerializer == null) throw new InvalidOperationException("Cannot AddJsonBody before InitializeRequestSerializer");

            request.Body = requestSerializer.SerializeBody(bodyObject);
            request.Headers.Add("Content-Type", "application/json");
        }
    }
}