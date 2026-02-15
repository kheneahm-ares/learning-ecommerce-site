using Catalog.Features.Products;

namespace Catalog.Extensions
{
    public static class EndpointExtensions
    {
        public static WebApplication MapCatalogEndpoints(this WebApplication app)
        {
            var catalog = app.MapGroup("/api/catalog");

            var products = catalog.MapGroup("/products");
            products.MapProductsEndpoints();

            return app;
        }
    }
}
