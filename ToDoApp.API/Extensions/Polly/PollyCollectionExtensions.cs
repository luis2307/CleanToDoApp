using Microsoft.Extensions.Caching.Memory;
using Polly.Extensions.Http;
using Polly;
using Polly.Caching.Memory;

namespace ToDoApp.API.Extensions.Polly
{
    public static class PollyCollectionExtensions
    {
        public static IServiceCollection AddResilientHttpClient(this IServiceCollection services, string clientName)
        {
            services.AddHttpClient(clientName)
                  //.AddPolicyHandler(GetRetryPolicy())
                  .AddPolicyHandler(GetRateLimiterPolicy())
                 // .AddPolicyHandler(GetTimeoutPolicy())
                 // .AddPolicyHandler((sp, request) => GetCachePolicy(sp))
                  ;

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            // Configurar el tiempo de espera de 5 segundos
            return Policy.TimeoutAsync<HttpResponseMessage>(5);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            /*
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            */

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRateLimiterPolicy()
        {
            return Policy.RateLimitAsync<HttpResponseMessage>(3, TimeSpan.FromSeconds(10));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCachePolicy(IServiceProvider services)
        {
            var cacheProvider = services.GetRequiredService<IMemoryCache>();
            var memoryCacheProvider = new MemoryCacheProvider(cacheProvider);

            return Policy.CacheAsync<HttpResponseMessage>(memoryCacheProvider, TimeSpan.FromSeconds(10));

        }
    }
}
