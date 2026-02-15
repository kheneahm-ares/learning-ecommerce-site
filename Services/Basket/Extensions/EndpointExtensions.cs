
using Basket.Features.Basket;

namespace Basket.Extensions
{
    public static class EndpointExtensions
    {
        public static WebApplication MapBasketEndpoints(this WebApplication app)
        {
            var catalog = app.MapGroup("/api/catalog");

            var basket = catalog.MapGroup("/basket");
            basket.MapBasketEndpoints();

            return app;
        }
    }
}
