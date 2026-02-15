using Basket.Entities;
using Basket.Features.Basket.DTOs;

namespace Basket.Features.Shared.Mappers
{
    public static class ShoppingCartMapper
    {
        // to entity from createshoppingcartitemrequest
        public static ShoppingCart ToEntity(this CreateShoppingCartRequest request)
        {
            return new ShoppingCart
            {
                UserName = request.Username,
                Items = request.Items
            };
        }

        // to response from shoppingcart
        public static ShoppingCartResponse ToResponse(this ShoppingCart cart)
        {
            return new ShoppingCartResponse
            {
                UserName = cart.UserName,
                Items = cart.Items
            };
        }
    }
}
