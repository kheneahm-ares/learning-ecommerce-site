
using Basket.Features.Basket;

namespace Basket.Extensions
{
    public static class ServicesExtensions
    {

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // redis cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["CacheSettings:ConnectionString"];
            });

            // features
            services.AddScoped<BasketService>();

            return services;
        }
    }
}
