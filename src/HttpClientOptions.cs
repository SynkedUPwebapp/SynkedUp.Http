namespace EL.Http
{
    public class HttpClientOptions
    {
        public int DefaultTimeoutMilliseconds { get; set; }
        public bool AllowAutoRedirect { get; set; }

        public HttpClientOptions()
        {
            DefaultTimeoutMilliseconds = 0;
            AllowAutoRedirect = true;
        }
    }
}