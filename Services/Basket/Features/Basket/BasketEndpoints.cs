using Basket.Features.Basket.DTOs;

namespace Basket.Features.Basket
{
    public static class BasketEndpoints
    {

        public static RouteGroupBuilder MapBasketEndpoints(this RouteGroupBuilder group)
        {
            // get basket by username
            group.MapGet("/{username}", async (string username, BasketService service) => await service.GetBasket(username));

            // create or update basket
            group.MapPost("/", async (CreateShoppingCartRequest request, BasketService service) => await service.UpsertBasket(request));

            // delete basket by username
            group.MapDelete("/{username}", async (string username, BasketService service) => await service.DeleteBasket(username));

            return group;
        }
    }
}
