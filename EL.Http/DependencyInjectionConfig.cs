using Microsoft.Extensions.DependencyInjection;

namespace EL.Http
{
    public class DependencyInjectionConfig
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpClient, HttpClient>();
        }
    }
}