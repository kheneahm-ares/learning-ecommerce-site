using Basket.Entities;

namespace Basket.Features.Basket.DTOs
{
    public class CreateShoppingCartRequest
    {
        public string Username { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
    }
}
