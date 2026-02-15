using Catalog.Features.ProductBrands;
using Catalog.Features.Products;
using Catalog.Features.ProductTypes;
using MongoDB.Driver;

namespace Catalog.Extensions
{
    public static class ServicesExtensions
    {

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // mongodb
            services.AddSingleton<IMongoClient>(s =>
            {
                var connectionString = configuration["DatabaseSettings:ConnectionString"];
                return new MongoClient(connectionString);
            });

            // mongodb with database
            services.AddScoped<IMongoDatabase>(services =>
            {
                var client = services.GetRequiredService<IMongoClient>();
                var databaseName = configuration["DatabaseSettings:DatabaseName"];
                return client.GetDatabase(databaseName);
            });


            // features
            services.AddScoped<ProductsService>();
            services.AddScoped<ProductTypesServices>();
            services.AddScoped<ProductBrandsService>();

            return services;
        }
    }
}
